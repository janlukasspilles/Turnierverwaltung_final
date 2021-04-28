//Autor:        Jan-Lukas Spilles
//Klasse:       IA119
//Datei:        CustomTable.cs
//Datum:        13.03.2021
//Beschreibung: Unterklasse von Table, zum dynamischen Anzeigen von Daten.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using Turnierverwaltung.Model;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;

namespace Turnierverwaltung_final.Helper
{
    public class CustomTable : Table
    {
        #region Attributes
        private List<Teilnehmer> _content;
        private List<PropertyInfo> _displayFields;
        private Type _listDataType;
        private Button _deleteButton;
        private Button _cancelButton;
        private Button _addButton;
        private Button _submitButton;
        #endregion
        #region Properties
        public List<Teilnehmer> Content
        {
            get => _content;
            set
            {
                _content = value;
            }
        }
        public List<PropertyInfo> DisplayFields { get => _displayFields; set => _displayFields = value; }
        public Type ListDataType
        {
            get => _listDataType;
            set
            {
                _listDataType = value;
            }
        }

        public bool HasNewEntry
        {
            get
            {
                if (ViewState["HasNewEntry"] == null)
                    return false;
                return (bool)ViewState["HasNewEntry"];
            }
            set => ViewState["HasNewEntry"] = value;
        }

        public Button DeleteButton { get => _deleteButton; set => _deleteButton = value; }
        public Button CancelButton { get => _cancelButton; set => _cancelButton = value; }
        public Button AddButton { get => _addButton; set => _addButton = value; }
        public Button SubmitButton { get => _submitButton; set => _submitButton = value; }
        #endregion
        #region Constructors
        public CustomTable(List<Teilnehmer> content, string tableId) : base()
        {            
            CssClass = "table table-bordered";
            DisplayFields = new List<PropertyInfo>();
            Content = content;
            ID = tableId;
            if (Content != null && Content.Count > 0)
            {
                ListDataType = GetListDatatype();
                if (ListDataType != null)
                {
                    foreach (PropertyInfo propertyInfo in ListDataType.GetProperties())
                    {
                        if (Attribute.IsDefined(propertyInfo, typeof(DisplayAttribute)))
                            DisplayFields.Add(propertyInfo);
                    }
                    DisplayFields = DisplayFields.OrderBy(p => (p.GetCustomAttribute(typeof(DisplayAttribute), true) as DisplayAttribute).Order).ToList();
                }
            }
            CreateTable();
        }
        #endregion
        #region Methods
        public void CreateTable()
        {
            Rows.Clear();
            GenerateHeaderRow();
            if (Content != null && Content.Count > 0)
                GenerateDataRows();
            GenerateButtonRow();
        }
        private void GenerateHeaderRow()
        {
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (PropertyInfo shownPropertyInfo in DisplayFields)
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
                Rows.Add(headerRow);
            }
        }

        private void GenerateDataRows()
        {
            for (int memberNum = 0; memberNum < Content.Count; memberNum++)
            {
                // Wir geben MemberNum + 1, damit die Id der Row gleich der Position des Datensatzes
                // in der Liste ist, da in es noch eine HeaderRows gibt.
                CustomRow newRow;
                if (Content[memberNum].Id != 0)
                    newRow = new CustomRow(Content[memberNum], memberNum + 1, DisplayFields);
                else
                    newRow = new CustomRow(Content[memberNum], memberNum + 1, DisplayFields, RowState.rsInsert);
                Rows.Add(newRow);
            }
        }

        public void GenerateNewEntryRow()
        {
            CustomRow newRow = new CustomRow(null, Rows.Count, DisplayFields, RowState.rsInsert);
            Rows.AddAt(Rows.Count - 2, newRow);
        }

        private void GenerateButtonRow()
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            //Submit Button
            SubmitButton = new Button()
            {
                Text = "Änderungen speichern",
                Visible = true,
                CssClass = "btn btn-secondary",
                ID = "btnAccept",
            };
            SubmitButton.Click += acceptButton_Click;
            tc.Controls.Add(SubmitButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Delete Button
            DeleteButton = new Button()
            {
                Text = "Ausgewählte löschen",
                Visible = true,
                ID = "btnDelete",
                CssClass = "btn btn-secondary",
            };
            DeleteButton.Click += deleteButton_Click;
            tc.Controls.Add(DeleteButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Cancel Button
            CancelButton = new Button()
            {
                Text = "Änderungen verwerfen",
                Visible = true,
                ID = "btnCancel",
                CssClass = "btn btn-secondary",
            };
            CancelButton.Click += cancelButton_Click;
            tc.Controls.Add(CancelButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Add Button
            AddButton = new Button()
            {
                Text = "Hinzufügen",
                Visible = true,
                ID = "btnAdd",
                CssClass = "btn btn-secondary",
            };
            AddButton.Click += addButton_Click;
            tc.Controls.Add(AddButton);
            tr.Cells.Add(tc);

            Rows.Add(tr);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Teilnehmer t = (Teilnehmer)Activator.CreateInstance(ListDataType);
            Content.Add(t);
            HasNewEntry = true;
            CreateTable();
        }

        private Type GetListDatatype()
        {
            Type t = Content.First().GetType();
            if (!Content.All(x => x.GetType().Equals(t)))
            {
                //Checken, ob es eine gemeinsame Basisklasse gibt
                for (Type current = t; current != null; current = current.BaseType)
                {
                    if (Content.All(x => x.GetType().IsSubclassOf(current)))
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

        //Accept-Button OnClickEvent
        private void acceptButton_Click(object sender, EventArgs e)
        {
            foreach (TableRow tmp in Rows)
            {
                if (tmp is CustomRow)
                {
                    if ((tmp as CustomRow).RowState == RowState.rsEdit)
                    {
                        //(tmp as CustomRow).InEditMode = false;
                        for (int i = 0; i < DisplayFields.Count; i++)
                        {
                            var cur = Content[Rows.GetRowIndex(tmp) - 1];
                            //Problem: Zu diesem Zeitpunkt handelt es sich hier um ein Label                         
                            ListDataType.GetProperty(DisplayFields[i].Name).SetValue(Content[Rows.GetRowIndex(tmp) - 1], Convert.ChangeType((tmp.Cells[i].Controls[0] as TextBox).Text, DisplayFields[i].PropertyType));
                            Content[Rows.GetRowIndex(tmp) - 1].Speichern();
                        }
                        (tmp as CustomRow).RowState = RowState.rsNone;
                        (tmp as CustomRow).RefreshRow();
                    }
                    else if ((tmp as CustomRow).RowState == RowState.rsInsert)
                    {
                        for (int i = 0; i < DisplayFields.Count; i++)
                        {
                            var cur = Content[Rows.GetRowIndex(tmp) - 1];
                            if (DisplayFields[i].Name != "Id")
                            {
                                ListDataType.GetProperty(DisplayFields[i].Name).SetValue(Content[Rows.GetRowIndex(tmp) - 1], Convert.ChangeType((tmp.Cells[i].Controls[0] as TextBox).Text, DisplayFields[i].PropertyType));
                            }
                        }
                        Content[Rows.GetRowIndex(tmp) - 1].Neuanlage();
                        (tmp as CustomRow).RowState = RowState.rsNone;
                        (tmp as CustomRow).RefreshRow();
                    }
                }
            }
        }

        //Delete-Button OnClickEvent
        private void deleteButton_Click(object sender, EventArgs e)
        {
            List<TableRow> deleteRows = new List<TableRow>();
            foreach (TableRow tmp in Rows)
            {
                if (tmp is CustomRow)
                {
                    if ((tmp as CustomRow).SelectedCheckBox.Checked)
                    {
                        Content[Rows.GetRowIndex(tmp) - 1].Loeschen();
                        deleteRows.Add(tmp);
                    }
                }
            }
            //Rows müssen im nachhinein gelöscht werden, da sonst Exception bei foreach
            foreach (TableRow delRow in deleteRows)
            {
                Rows.Remove(delRow);
            }
        }

        //Cancel-Button OnClick
        private void cancelButton_Click(object sender, EventArgs e)
        {
            foreach (TableRow tmp in Rows)
            {
                if (tmp is CustomRow)
                {
                    if ((tmp as CustomRow).RowState == RowState.rsEdit)
                    {
                        (tmp as CustomRow).RowState = RowState.rsNone;
                        (tmp as CustomRow).RefreshRow();
                    }
                }
            }
        }
        #endregion
    }
}