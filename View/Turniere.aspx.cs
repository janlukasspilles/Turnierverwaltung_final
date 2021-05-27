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
using Turnierverwaltung_final.Model.TurniereNS;
// Es gibt eine Tabelle mit allen Mannschaften analog zur Personenansicht
// Es gibt einen neuen Button "Spieler hinzufügen" in der Tabelle für jede Mannschaft
// Wird dieser Button gedrückt öffnen sich zwei Felder
// Feld1 sind die Mitglieder der entsprechenden Mannschaft
// Feld2 zeigt alle Personen, die Mitglieder dieser Mannschaft werden können
// Spieler in den jeweiligen Feldern lassen sich auswählen
// Zwei Buttons über die Spieler hinzugefügt oder entfernt werden können
namespace Turnierverwaltung_final.View
{
    public partial class Turniere : Page
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
        #endregion
        #region Constructors
        public Turniere() : base()
        {
            
        }
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            Controller.GetAlleTurniere();
            LoadTable();
        }
        private void LoadTable()
        {
            tbl_turniere.Rows.Clear();

            Type ListDataType = Controller.Turniere.Count > 0 ? GetListDatatype(Controller.Turniere) : Type.GetType($"Turnierverwaltung_final.Model.Turniere.Turnier");
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);

            //Headerrow
            tbl_turniere.Rows.Add(GetHeaderRow(DisplayFields));

            //Datarows
            for (int memberNum = 0; memberNum < Controller.Turniere.Count; memberNum++)
            {
                tbl_turniere.Rows.Add(GetDataRow(Controller.Turniere[memberNum], DisplayFields, memberNum + 1, memberNum + 1 == EditableRow));
            }
            //Es wird ein neuer Teilnehmer hinzugefügt
            if (Controller.NeuesTurnier != null)
            {
                tbl_turniere.Rows.Add(GetDataRow(Controller.NeuesTurnier, DisplayFields, Controller.Turniere.Count + 1, true));
            }

            //Footerrow
            if (DisplayFields.Count > 0)
                tbl_turniere.Rows.Add(GetFooterRow());

            //Unterer Teil
            if (EditableRow != -1)
            {
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
        private TableRow GetDataRow(Turnier T, List<PropertyInfo> displayFields, int pos, bool editable)
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
                    switch (dmi.ControlType)
                    {
                        case ControlType.ctEditText:
                            newControl = new Label() { ID = $"con{counter}Row{pos}" };
                            (newControl as Label).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                            break;
                        case ControlType.ctDomain:
                            newControl = new Label() { ID = $"con{counter}Row{pos}" };
                            //Hack -> muss umgebaut werden, da Ergebnis von Reihenfolge der Selektion abhängt
                            (newControl as Label).Text = Controller.GetDomainList(dmi.DomainList)[Convert.ToInt32(T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "") - 1].ToString();
                            break;
                        case ControlType.ctCheck:
                            newControl = new CheckBox() { ID = $"con{counter}Row{pos}" };
                            (newControl as CheckBox).Checked = (bool)T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null);
                            (newControl as CheckBox).Enabled = false;
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
            if (EditableRow == -1)
            {
                if (!T.Abgeschlossen)
                {
                    Button TurnierDurchfuehren = new Button
                    {
                        ID = $"btnDurchfuehren{pos}",
                        Text = $"Turnier durchführen"
                    };
                    newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                    newCell.Controls.Add(TurnierDurchfuehren);
                    tr.Cells.Add(newCell);
                }
                else
                {
                    Button ZeigeSpiele = new Button
                    {
                        ID = $"btnZeigeSpiele{pos}",
                        Text = $"Zeige Spiele"
                    };
                    newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                    newCell.Controls.Add(ZeigeSpiele);
                    tr.Cells.Add(newCell);
                }
                //Edit-Button
                newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };                
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
        
        private void OnEditButton_Click(object sender, CommandEventArgs e)
        {
            EditableRow = Convert.ToInt32(e.CommandArgument);
            LoadTable();
        }
        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            foreach (TableRow tmp in tbl_turniere.Rows)
            {
                if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{tbl_turniere.Rows.GetRowIndex(tmp)}") is CheckBox cb && cb.Checked)
                {
                    Turnier t = Controller.Turniere[tbl_turniere.Rows.GetRowIndex(tmp) - 1];
                    t.Loeschen();
                    Controller.Turniere.Remove(t);
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
            Type ListDataType = GetListDatatype(Controller.Turniere);
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);
            Turnier t = Controller.NeuesTurnier ?? Controller.Turniere[EditableRow - 1];

            for (int i = 0; i < DisplayFields.Count; i++)
            {
                DisplayMetaInformation dmi = DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                if (dmi.Editable)
                {
                    switch (dmi.ControlType)
                    {
                        case ControlType.ctEditText:
                            ListDataType.GetProperty(DisplayFields[i].Name).SetValue(t, Convert.ChangeType((tbl_turniere.Rows[EditableRow].Cells[i].Controls[0] as TextBox).Text, DisplayFields[i].PropertyType));
                            break;
                        case ControlType.ctDomain:
                            int domainId = (tbl_turniere.Rows[EditableRow].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                            ListDataType.GetProperty(DisplayFields[i].Name).SetValue(t, Convert.ChangeType(domainId, DisplayFields[i].PropertyType));
                            break;
                    }
                }
            }
            //Insertlogik
            if (Controller.NeuesTurnier != null)
            {
                t.Neuanlage();
                Controller.Turniere.Add(t);
                Controller.NeuesTurnier = null;
            }
            //Updatelogik
            else
            {                
                t.Speichern();
            }

            EditableRow = -1;
            LoadTable();
        }
        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Type listDataType = Controller.Turniere.Count > 0 ? GetListDatatype(Controller.Turniere) : Type.GetType($"Turnierverwaltung_final.Model.TurniereNS.Turnier");
            Turnier t = (Turnier)Activator.CreateInstance(listDataType);
            Controller.NeuesTurnier = t;
            EditableRow = tbl_turniere.Rows.Count - 1;
            LoadTable();
        }
        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            Controller.Teilnehmer = Controller.Teilnehmer.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
        }
        private Type GetListDatatype(List<Turnier> content)
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
        #endregion
    }
}