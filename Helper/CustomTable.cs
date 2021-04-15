//Autor:        Jan-Lukas Spilles
//Klasse:       IA119
//Datei:        CustomTable.cs
//Datum:        13.03.2021
//Beschreibung: Unterklasse von Table, zum dynamischen Anzeigen von Daten.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung_final.Model.Spieler;

namespace Turnierverwaltung_final.Helper
{
    public class CustomTable : Table
    {
        #region Attributes
        private List<Teilnehmer> _content;
        private List<PropertyInfo> _displayFields;
        private Type _listDataType;
        private bool _editable;
        private int _editedRowIndex;
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

        public bool Editable { get => _editable; set => _editable = value; }
        public int EditedRowIndex { get => _editedRowIndex; set => _editedRowIndex = value; }
        #endregion
        #region Constructors
        public CustomTable(List<Teilnehmer> content) : base()
        {
            DisplayFields = new List<PropertyInfo>();
            Content = content;
            ID = "tbl_custom";
            if (Content != null && Content.Count > 0)
            {
                ListDataType = GetListDatatype();
                if (ListDataType != null)
                {
                    foreach (PropertyInfo propertyInfo in ListDataType.GetProperties())
                    {
                        if (!Attribute.IsDefined(propertyInfo, typeof(DisplayNameAttribute)))
                            DisplayFields.Add(propertyInfo);
                    }
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
                CustomRow newRow = new CustomRow(Content[memberNum], memberNum + 1, DisplayFields);
                Rows.Add(newRow);
            }
        }

        private void GenerateButtonRow()
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            //Accept Button
            Button btn = new Button()
            {
                Text = "Änderungen speichern",
                Visible = true,
                ID = "btnAccept",
            };
            btn.Click += acceptButton_Click;
            tc.Controls.Add(btn);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Delete Button
            btn = new Button()
            {
                Text = "Ausgewählte löschen",
                Visible = true,
                ID = "btnDelete",
            };
            btn.Click += deleteButton_Click;
            tc.Controls.Add(btn);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Cancel Button
            btn = new Button()
            {
                Text = "Änderungen verwerfen",
                Visible = true,
                ID = "btnCancel",
            };
            btn.Click += cancelButton_Click;
            tc.Controls.Add(btn);
            tr.Cells.Add(tc);

            Rows.Add(tr);
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
                    if ((tmp as CustomRow).InEditMode)
                    {
                        //(tmp as CustomRow).InEditMode = false;
                        for (int i = 0; i < DisplayFields.Count; i++)
                        {
                            var cur = Content[Rows.GetRowIndex(tmp) - 1];
                            //Hack: Irgendwie sind das zu diesem Zeitpunkt wieder Labels und keine Textboxen mehr...
                            //Doch ein Big-Mistake --> Durch den Controlwechsel geht der Wert flöten                            
                            ListDataType.GetProperty(DisplayFields[i].Name).SetValue(Content[Rows.GetRowIndex(tmp) - 1], Convert.ChangeType((tmp.Cells[i].Controls[0] as Label).Text, DisplayFields[i].PropertyType));
                        }
                        //Convert.ChangeType(Content[Rows.GetRowIndex(tmp) - 1], ListDataType).;
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
                    if ((tmp as CustomRow).InEditMode)
                    {
                        (tmp as CustomRow).InEditMode = false;
                        (tmp as CustomRow).RefreshRow();
                    }
                }
            }
        }
        #endregion
    }
}