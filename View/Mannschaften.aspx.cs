using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.Model.TeilnehmerNS;
using Turnierverwaltung_final.Helper;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;
using Turnierverwaltung_final.Model.TeilnehmerNS.Personen;
// Es gibt eine Tabelle mit allen Mannschaften analog zur Personenansicht
// Es gibt einen neuen Button "Spieler hinzufügen" in der Tabelle für jede Mannschaft
// Wird dieser Button gedrückt öffnen sich zwei Felder
// Feld1 sind die Mitglieder der entsprechenden Mannschaft
// Feld2 zeigt alle Personen, die Mitglieder dieser Mannschaft werden können
// Spieler in den jeweiligen Feldern lassen sich auswählen
// Zwei Buttons über die Spieler hinzugefügt oder entfernt werden können
namespace Turnierverwaltung_final.View
{
    public partial class Mannschaften : Page
    {
        #region Attributes
        private Controller _controller;
        private readonly string[] _ignoreFields = { };
        #endregion
        #region Properties
        public Controller Controller { get => _controller; set => _controller = value; }
        public int EditableRow
        {
            get
            {
                if (ViewState["InEdit"] == null)
                    return -1;
                return (int)ViewState["InEdit"];
            }
            set => ViewState["InEdit"] = value;
        }
        public List<Person> MoeglicheMitglieder
        {
            get
            {
                if (ViewState["MoeglicheMitglieder"] != null)
                    return (List<Person>)ViewState["MoeglicheMitglieder"];
                return null;
            }
            set => ViewState["MoeglicheMitglieder"] = value;
        }
        public List<Person> Mitglieder
        {
            get
            {
                if (ViewState["Mitglieder"] != null)
                    return (List<Person>)ViewState["Mitglieder"];
                return null;
            }
            set => ViewState["Mitglieder"] = value;
        }
        #endregion
        #region Constructors
        public Mannschaften() : base()
        {

        }
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            Controller.GetAlleMannschaften();
            LoadTable();
        }
        private void LoadTable()
        {
            tbl_mannschaften.Rows.Clear();

            Type ListDataType = Controller.Teilnehmer.Count > 0 ? GetListDatatype(Controller.Teilnehmer) : Type.GetType($"Turnierverwaltung_final.Model.Personen.Mannschaft");
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);

            //Headerrow
            tbl_mannschaften.Rows.Add(GetHeaderRow(DisplayFields));

            //Datarows
            for (int memberNum = 0; memberNum < Controller.Teilnehmer.Count; memberNum++)
            {
                tbl_mannschaften.Rows.Add(GetDataRow(Controller.Teilnehmer[memberNum], DisplayFields, memberNum + 1, memberNum + 1 == EditableRow));
            }
            //Es wird ein neuer Teilnehmer hinzugefügt
            if (Controller.NeuerTeilnehmer != null)
            {
                tbl_mannschaften.Rows.Add(GetDataRow(Controller.NeuerTeilnehmer, DisplayFields, Controller.Teilnehmer.Count + 1, true));
            }

            //Footerrow
            if (DisplayFields.Count > 0)
                tbl_mannschaften.Rows.Add(GetFooterRow());

            //Unterer Teil
            if (EditableRow != -1)
            {
                pnl_mitglieder.Controls.Clear();
                if (Controller.NeuerTeilnehmer == null)
                    pnl_mitglieder.Controls.Add(GetSwitchPanel());
            }
        }
        private TableHeaderRow GetHeaderRow(List<PropertyInfo> displayFields)
        {
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (PropertyInfo shownPropertyInfo in displayFields)
            {
                TableHeaderCell newHeaderCell = new TableHeaderCell();
                Button newBtn = new Button
                {
                    Text = (shownPropertyInfo.GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Displayname,
                    Width = Unit.Percentage(100),
                    BorderStyle = BorderStyle.None,
                    ID = $"btn{shownPropertyInfo.Name}"
                };
                newBtn.Style.Add("text-align", "left");
                newBtn.CommandArgument = shownPropertyInfo.Name;
                newBtn.Command += OnHeaderButton_Click;
                newHeaderCell.Controls.Add(newBtn);
                headerRow.Cells.Add(newHeaderCell);
            }
            return headerRow;
        }
        private TableRow GetDataRow(Teilnehmer T, List<PropertyInfo> displayFields, int pos, bool editable)
        {
            TableRow tr = new TableRow();
            TableCell newCell = null;
            for (int counter = 0; counter < displayFields.Count; counter++)
            {
                newCell = new TableCell() { ID = $"tblCell{counter}Row{pos}" };
                Control newControl = null;
                DisplayMetaInformation dmi = displayFields[counter].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                if (!editable || !(displayFields[counter].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Editable)
                {
                    newControl = new Label() { ID = $"con{counter}Row{pos}" };
                    switch (dmi.ControlType)
                    {
                        case ControlType.ctEditText:
                            (newControl as Label).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                            break;
                        case ControlType.ctDomain:
                            //Hack -> muss umgebaut werden, da Ergebnis von Reihenfolge der Selektion abhängt
                            (newControl as Label).Text = Controller.GetDomainList(dmi.DomainList)[Convert.ToInt32(T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "") - 1].ToString();
                            break;
                    }
                }
                else
                {
                    switch (dmi.ControlType)
                    {
                        case ControlType.ctEditText:
                            newControl = new TextBox() { ID = $"con{counter}Row{pos}", CssClass = "form-control" };
                            (newControl as TextBox).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                            break;
                        case ControlType.ctDomain:
                            newControl = new DropDownList { ID = $"con{counter}Row{pos}", CssClass = "form-control" };
                            (newControl as DropDownList).DataSource = Controller.GetDomainList(dmi.DomainList);
                            (newControl as DropDownList).DataBind();
                            //Hack -> muss umgebaut werden, da Ergebnis von Reihenfolge der Selektion abhängt
                            (newControl as DropDownList).SelectedIndex = Convert.ToInt32(T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "") - 1;
                            break;
                    }

                }

                newCell.Controls.Add(newControl);
                tr.Cells.Add(newCell);
            }
            newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
            if (EditableRow == -1)
            {
                //Edit-Button
                Button EditButton = new Button
                {
                    ID = $"btnEdit{pos}",
                    Text = "Editieren"
                };
                EditButton.Command += OnEditButton_Click;
                EditButton.CssClass = "btn btn-success";
                EditButton.CommandArgument = pos.ToString();
                newCell.Controls.Add(EditButton);
                tr.Cells.Add(newCell);
                //CheckBox
                newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                CheckBox SelectedCheckBox = new CheckBox() { ID = $"cbSelected{pos}" };
                newCell.Controls.Add(SelectedCheckBox);
                tr.Cells.Add(newCell);
            }
            return tr;
        }
        private TableFooterRow GetFooterRow()
        {
            TableFooterRow tr = new TableFooterRow();
            TableCell tc = new TableCell();

            if (EditableRow == -1)
            {
                tc = new TableCell();
                //Delete Button
                Button DeleteButton = new Button()
                {
                    Text = "Ausgewählte löschen",
                    Visible = true,
                    ID = "btnDelete",
                    CssClass = "btn btn-secondary",
                };
                DeleteButton.Click += OnDeleteButton_Click;
                tc.Controls.Add(DeleteButton);
                tr.Cells.Add(tc);

                tc = new TableCell();
                //Add Button
                Button AddButton = new Button()
                {
                    Text = "Hinzufügen",
                    Visible = true,
                    ID = "btnAddMannschaft",
                    CssClass = "btn btn-secondary",
                };
                AddButton.Click += OnAddButton_Click;
                tc.Controls.Add(AddButton);
                tr.Cells.Add(tc);
            }
            else
            {
                //Submit Button
                Button SubmitButton = new Button()
                {
                    Text = "Änderungen speichern",
                    Visible = true,
                    CssClass = "btn btn-secondary",
                    ID = "btnAccept",
                };
                SubmitButton.Click += OnSubmitButton_Click;
                tc.Controls.Add(SubmitButton);
                tr.Cells.Add(tc);

                tc = new TableCell();
                //Cancel Button
                Button CancelButton = new Button()
                {
                    Text = "Änderungen verwerfen",
                    Visible = true,
                    ID = "btnCancel",
                    CssClass = "btn btn-secondary",
                };
                CancelButton.Click += OnCancelButton_Click;
                tc.Controls.Add(CancelButton);
                tr.Cells.Add(tc);
            }
            return tr;
        }
        private Panel GetSwitchPanel()
        {
            Panel result = new Panel();
            //Headline
            HtmlGenericControl headline = new HtmlGenericControl("h2")
            {
                InnerText = $"Mitgliederliste für {(Controller.Teilnehmer[EditableRow - 1] as Mannschaft).Name}",
                ID = "headlineMitgliederSwitch"
            };
            result.Controls.Add(headline);

            //Switcher
            Panel switcher = new Panel
            {
                ID = "switcherPanel"
            };

            ListBox lb = new ListBox
            {
                ID = "lbMitglieder",
                CssClass = "form-control",
                DataSource = Mitglieder
            };
            lb.DataBind();
            switcher.Controls.Add(lb);

            Button btn = new Button
            {
                CssClass = "btn",
                Text = "Add",
                ID = "btnAddMitglied"
            };
            btn.Click += OnAddMitgliedButton_Click;
            switcher.Controls.Add(btn);

            btn = new Button
            {
                CssClass = "btn",
                Text = "Remove",
                ID = "btnRemoveMitglied"
            };
            btn.Click += OnRemoveMitgliedButton_Click;
            switcher.Controls.Add(btn);

            lb = new ListBox
            {
                ID = "lbMoeglicheMitglieder",
                CssClass = "form-control",
                DataSource = MoeglicheMitglieder
            };
            lb.DataBind();
            switcher.Controls.Add(lb);

            result.Controls.Add(switcher);
            return result;
        }
        private void OnEditButton_Click(object sender, CommandEventArgs e)
        {
            EditableRow = Convert.ToInt32(e.CommandArgument);
            Mannschaft mannschaftInEdit = (Controller.Teilnehmer[EditableRow - 1] as Mannschaft);
            //mannschaftInEdit.SelektiereMitgliederListe();
            Mitglieder = mannschaftInEdit.Mitglieder;
            MoeglicheMitglieder = Controller.GetMoeglicheMitglieder(mannschaftInEdit.Id);
            LoadTable();
        }
        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            foreach (TableRow tmp in tbl_mannschaften.Rows)
            {
                if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{tbl_mannschaften.Rows.GetRowIndex(tmp)}") is CheckBox cb && cb.Checked)
                {
                    Teilnehmer t = Controller.Teilnehmer[tbl_mannschaften.Rows.GetRowIndex(tmp) - 1];
                    t.Loeschen();
                    Controller.Teilnehmer.Remove(t);
                }
            }
            LoadTable();
        }
        private void OnCancelButton_Click(object sender, EventArgs e)
        {
            //Wurde Hinzugefügt wird das hier Rückgängig gemacht
            Controller.NeuerTeilnehmer = null;
            //Es gibt keine Editrow mehr
            EditableRow = -1;
            LoadTable();
        }
        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            Type ListDataType = GetListDatatype(Controller.Teilnehmer);
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);
            Teilnehmer t = Controller.NeuerTeilnehmer ?? Controller.Teilnehmer[EditableRow - 1];

            for (int i = 0; i < DisplayFields.Count; i++)
            {
                DisplayMetaInformation dmi = DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                if (dmi.Editable)
                {
                    switch (dmi.ControlType)
                    {
                        case ControlType.ctEditText:
                            ListDataType.GetProperty(DisplayFields[i].Name).SetValue(t, Convert.ChangeType((tbl_mannschaften.Rows[EditableRow].Cells[i].Controls[0] as TextBox).Text, DisplayFields[i].PropertyType));
                            break;
                        case ControlType.ctDomain:
                            int domainId = (tbl_mannschaften.Rows[EditableRow].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                            ListDataType.GetProperty(DisplayFields[i].Name).SetValue(t, Convert.ChangeType(domainId, DisplayFields[i].PropertyType));
                            break;
                    }
                }
            }
            //Insertlogik
            if (Controller.NeuerTeilnehmer != null)
            {
                t.Neuanlage();
                Controller.Teilnehmer.Add(t);
                Controller.NeuerTeilnehmer = null;
            }
            //Updatelogik
            else
            {
                (t as Mannschaft).Mitglieder = Mitglieder;
                t.Speichern();
            }

            EditableRow = -1;
            LoadTable();
        }
        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Type listDataType = Controller.Teilnehmer.Count > 0 ? GetListDatatype(Controller.Teilnehmer) : Type.GetType($"Turnierverwaltung_final.Model.Personen.Mannschaft");
            Teilnehmer t = (Teilnehmer)Activator.CreateInstance(listDataType);
            Controller.NeuerTeilnehmer = t;
            EditableRow = tbl_mannschaften.Rows.Count - 1;
            LoadTable();
        }
        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            Controller.Teilnehmer = Controller.Teilnehmer.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
        }
        private Type GetListDatatype(List<Teilnehmer> content)
        {
            Type t = content.First().GetType();
            if (!content.All(x => x.GetType().Equals(t)))
            {
                //Checken, ob es eine gemeinsame Basisklasse gibt
                for (Type current = t; current != null; current = current.BaseType)
                {
                    if (content.All(x => x.GetType().IsSubclassOf(current)))
                    {
                        return current;
                    }
                }
                //Keine gemeinsame Basisklasse
                return null;
            }
            else
            {
                return t;
            }
        }
        private List<PropertyInfo> GetDisplayFields(Type ListDataType)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            if (ListDataType != null)
            {
                foreach (PropertyInfo propertyInfo in ListDataType.GetProperties())
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(DisplayMetaInformation)) && !_ignoreFields.Any(propertyInfo.Name.Contains))
                        result.Add(propertyInfo);
                }
                result = result.OrderBy(p => (p.GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Order).ToList();
            }
            return result;
        }
        private void OnAddMitgliedButton_Click(object sender, EventArgs e)
        {
            ListBox tmp = (((sender as Button).Parent as Panel).FindControl("lbMoeglicheMitglieder") as ListBox);
            if (tmp.SelectedIndex != -1)
                MoeglicheMitglieder.MoveTo(MoeglicheMitglieder[tmp.SelectedIndex], Mitglieder);
            LoadTable();
        }
        private void OnRemoveMitgliedButton_Click(object sender, EventArgs e)
        {
            ListBox tmp = (((sender as Button).Parent as Panel).FindControl("lbMitglieder") as ListBox);
            if (tmp.SelectedIndex != -1)
                Mitglieder.MoveTo(Mitglieder[tmp.SelectedIndex], MoeglicheMitglieder);
            LoadTable();
        }
        #endregion
    }
}