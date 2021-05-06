using System;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung_final.Helper;
using System.Linq;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;
using System.Web.UI;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Turnierverwaltung.Model;
using Turnierverwaltung.Model.TeilnehmerNS;

namespace Turnierverwaltung_final.View
{
    public partial class Personen : Page
    {
        #region Attributes
        private Controller _controller;
        private readonly string[] _ignoreFields = { "Name" };
        #endregion
        #region Properties
        public Controller Controller { get => _controller; set => _controller = value; }
        public int InEdit
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
        public Personen() : base()
        {

        }
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            LoadTable();
        }
        private void LoadTable()
        {
            tbl_personen.Rows.Clear();

            Type ListDataType = Controller.Teilnehmer.Count > 0 ? GetListDatatype(Controller.Teilnehmer) : Type.GetType($"Turnierverwaltung_final.Model.Personen.{ddl_selection.SelectedValue}");
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);

            //Headerrow
            tbl_personen.Rows.Add(GetHeaderRow(DisplayFields));

            //Datarows
            for (int memberNum = 0; memberNum < Controller.Teilnehmer.Count; memberNum++)
            {
                tbl_personen.Rows.Add(GetDataRow(Controller.Teilnehmer[memberNum], DisplayFields, memberNum + 1, memberNum + 1 == InEdit));
            }
            //Es wird ein neuer Teilnehmer hinzugefügt
            if (Controller.NeuerTeilnehmer != null)
            {
                tbl_personen.Rows.Add(GetDataRow(Controller.NeuerTeilnehmer, DisplayFields, Controller.Teilnehmer.Count + 1, true));
            }

            //Footerrow
            if (DisplayFields.Count > 0)
                tbl_personen.Rows.Add(GetFooterRow());
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
                newCell.Style.Add("text-align", "left");
                newCell.Style.Add("vertical-align", "middle");
                Control newControl = null;
                if (!editable || !(displayFields[counter].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Editable)
                {
                    newControl = new Label() { ID = $"con{counter}Row{pos}" };
                    ((Label)newControl).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                }
                else
                {
                    newControl = new TextBox() { ID = $"con{counter}Row{pos}", CssClass = "form-control" };
                    (newControl as TextBox).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                }
                newCell.Controls.Add(newControl);
                tr.Cells.Add(newCell);
            }            
            if (InEdit == -1)
            {
                newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                newCell.Style.Add("text-align", "left");
                newCell.Style.Add("vertical-align", "middle");
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
                newCell.Style.Add("text-align", "left");
                newCell.Style.Add("vertical-align", "middle");
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

            if (InEdit == -1)
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
                    ID = "btnAdd",
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
            InEdit = Convert.ToInt32(e.CommandArgument);
            LoadTable();
        }
        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            foreach (TableRow tmp in tbl_personen.Rows)
            {
                if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{tbl_personen.Rows.GetRowIndex(tmp)}") is CheckBox cb && cb.Checked)
                {
                    Teilnehmer t = Controller.Teilnehmer[tbl_personen.Rows.GetRowIndex(tmp) - 1];
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
            InEdit = -1;
            LoadTable();
        }
        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            //Bei der Neuanlage wird zunächst der Teilnehmer der Datenbank
            //hinzugefügt. Anschließend wird dieser Datensatz wie ein normal
            //editierter behandelt und mittels Update werden die Werte über-
            //tragen.
            if (Controller.NeuerTeilnehmer != null)
            {
                Controller.NeuerTeilnehmer.Neuanlage();
                Controller.Teilnehmer.Add(Controller.NeuerTeilnehmer);
                Controller.NeuerTeilnehmer = null;
            }
            //Updatelogik
            Type ListDataType = GetListDatatype(Controller.Teilnehmer);
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);

            Teilnehmer toBeUpdated = Controller.Teilnehmer[InEdit - 1];
            for (int i = 0; i < DisplayFields.Count; i++)
                if ((DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Editable)
                    ListDataType.GetProperty(DisplayFields[i].Name).SetValue(toBeUpdated, Convert.ChangeType((tbl_personen.Rows[InEdit].Cells[i].Controls[0] as TextBox).Text, DisplayFields[i].PropertyType));
            toBeUpdated.Speichern();

            InEdit = -1;
            LoadTable();
        }
        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Type listDataType = Controller.Teilnehmer.Count > 0 ? GetListDatatype(Controller.Teilnehmer) : Type.GetType($"Turnierverwaltung_final.Model.Personen.{ddl_selection.SelectedValue}");
            Teilnehmer t = (Teilnehmer)Activator.CreateInstance(listDataType);
            Controller.NeuerTeilnehmer = t;
            InEdit = tbl_personen.Rows.Count - 1;
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
        protected void OnSelectionChanged(object sender, EventArgs e)
        {
            switch (ddl_selection.SelectedValue)
            {
                case "Alle":
                    Controller.GetAllePersonen();
                    break;
                case "Trainer":
                    Controller.GetAlleTrainer();
                    break;
                case "Physio":
                    Controller.GetAllePhysios();
                    break;
                case "Fussballspieler":
                    Controller.GetAlleFussballspieler();
                    break;
                case "Handballspieler":
                    Controller.GetAlleHandballspieler();
                    break;
                case "Tennisspieler":
                    Controller.GetAlleTennisspieler();
                    break;
            }
            LoadTable();
        }
        #endregion        
    }
}