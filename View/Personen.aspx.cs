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

namespace Turnierverwaltung_final.View
{
    public partial class Personen : Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;

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
            if (Controller.Teilnehmer.Count > 0)
            {
                LoadTable();
            }
        }

        private void LoadTable()
        {
            tbl_persontest.Rows.Clear();
            Type ListDataType = GetListDatatype(Controller.Teilnehmer);
            List<PropertyInfo> DisplayFields = GetDisplayFields(ListDataType);

            //Headerrow
            tbl_persontest.Rows.Add(GetHeaderRow(DisplayFields));

            //Datarows
            for (int memberNum = 0; memberNum < Controller.Teilnehmer.Count; memberNum++)
            {
                tbl_persontest.Rows.Add(GetDataRow(Controller.Teilnehmer[memberNum], DisplayFields, memberNum + 1, false));
            }
            //Hinzufügen wurde gedrückt
            if (Controller.NeuerTeilnehmer != null)
            {
                tbl_persontest.Rows.Add(GetDataRow(Controller.NeuerTeilnehmer, DisplayFields, Controller.Teilnehmer.Count + 1, true));
            }

            //Footerrow
            tbl_persontest.Rows.Add(GetFooterRow());
        }
        private TableHeaderRow GetHeaderRow(List<PropertyInfo> displayFields)
        {
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (PropertyInfo shownPropertyInfo in displayFields)
            {
                TableHeaderCell newHeaderCell = new TableHeaderCell();
                Button newBtn = new Button();
                newBtn.Text = shownPropertyInfo.Name;
                newBtn.Width = Unit.Percentage(100);
                newBtn.BorderStyle = BorderStyle.None;
                newBtn.ID = $"btn{shownPropertyInfo.Name}";
                newBtn.Style.Add("text-align", "left");
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
                if (!editable)
                {
                    newControl = new Label() { ID = $"lbl{counter}Row{pos}" };
                    (newControl as Label).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                }
                else
                {
                    newControl = new TextBox() { ID = $"txt{counter}Row{pos}" };
                    (newControl as TextBox).Text = T.GetType().GetProperty(displayFields[counter].Name).GetValue(T, null)?.ToString() ?? "";
                }
                newCell.Controls.Add(newControl);
                tr.Cells.Add(newCell);
            }
            newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
            //Edit-Button
            Button EditButton = new Button();
            EditButton.ID = $"btnEdit{pos}";
            EditButton.Text = "Editieren";
            EditButton.Click += OnEditButton_Click;
            EditButton.CssClass = "btn btn-success";
            EditButton.CommandArgument = pos.ToString();
            newCell.Controls.Add(EditButton);
            tr.Cells.Add(newCell);
            //CheckBox
            newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
            CheckBox SelectedCheckBox = new CheckBox() { ID = $"cbSelected{pos}" };
            newCell.Controls.Add(SelectedCheckBox);
            tr.Cells.Add(newCell);
            return tr;
        }
        private TableFooterRow GetFooterRow()
        {
            TableFooterRow tr = new TableFooterRow();
            TableCell tc = new TableCell();

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
            return tr;
        }
        private void OnEditButton_Click(object sender, EventArgs e)
        {
            SetRowEditable(tbl_persontest.Rows[Convert.ToInt32((sender as Button).CommandArgument)]);
        }
        private void SetRowEditable(TableRow tr)
        {
            foreach (TableCell tc in tr.Cells)
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is Label label)
                    {
                        TextBox txtTmp = new TextBox()
                        {
                            ID = label.ID.Replace("lbl", "txt"),
                            Text = label.Text,
                            CssClass = "form-control",
                        };
                        tc.Controls.Remove(c);
                        tc.Controls.Add(txtTmp);
                    }
                }
            }
        }
        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            foreach (TableRow tmp in tbl_persontest.Rows)
            {
                if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{tbl_persontest.Rows.GetRowIndex(tmp)}") is CheckBox cb && cb.Checked)
                {
                    Teilnehmer t = Controller.Teilnehmer[tbl_persontest.Rows.GetRowIndex(tmp) - 1];
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
            LoadTable();
        }
        private void OnSubmitButton_Click(object sender, EventArgs e)
        {

            LoadTable();
        }
        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Type listDataType = GetListDatatype(Controller.Teilnehmer);
            Teilnehmer t = (Teilnehmer)Activator.CreateInstance(listDataType);
            Controller.NeuerTeilnehmer = t;
            LoadTable();
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
                    if (Attribute.IsDefined(propertyInfo, typeof(DisplayAttribute)))
                        result.Add(propertyInfo);
                }
                result = result.OrderBy(p => (p.GetCustomAttribute(typeof(DisplayAttribute), true) as DisplayAttribute).Order).ToList();
            }
            return result;
        }
    }
}