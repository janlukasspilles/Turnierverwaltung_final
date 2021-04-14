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
                if (CheckContentListValid())
                {
                    ListDataType = GetListDatatype();
                    if (ListDataType != null)
                        Refresh();
                }
            }
        }
        public List<PropertyInfo> DisplayFields { get => _displayFields; set => _displayFields = value; }
        public Type ListDataType
        {
            get => _listDataType;
            set
            {
                _listDataType = value;
                SetDisplayFields();
            }
        }

        public bool Editable { get => _editable; set => _editable = value; }
        public int EditedRowIndex { get => _editedRowIndex; set => _editedRowIndex = value; }
        #endregion
        #region Constructors
        public CustomTable()
        {
            DisplayFields = new List<PropertyInfo>();
            Content = new List<Teilnehmer>();
            ID = "tbl_custom";
        }
        public CustomTable(List<Teilnehmer> content)
        {
            DisplayFields = new List<PropertyInfo>();
            Content = content;
            ID = "tbl_custom";
        }
        #endregion
        #region Methods
        private bool CheckContentListValid()
        {
            return Content != null && Content.Count > 0;
        }

        /// <summary>
        /// Ermittelt die gemeinsame Basisklasse der Elemente.
        /// </summary>
        /// <returns>
        /// Typ der gemeinsamen Basisklasse der Elemente,
        /// gibt es keine wird null zurückgegeben.
        /// </returns>
        private Type GetListDatatype()
        {
            if (!Content.All(x => x.GetType().Equals(Content.First().GetType())))
            {
                //Checken, ob es eine gemeinsame Basisklasse gibt
                for (Type current = Content.First().GetType(); current != null; current = current.BaseType)
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
                return Content.First().GetType();
            }
        }

        /// <summary>
        /// Ermittelt die anzeigbaren Felder.
        /// </summary>
        private void SetDisplayFields()
        {
            foreach (PropertyInfo propertyInfo in ListDataType.GetProperties())
            {
                if (!Attribute.IsDefined(propertyInfo, typeof(DisplayNameAttribute)))
                    DisplayFields.Add(propertyInfo);
            }
        }

        /// <summary>
        /// Aktualisiert die Tabelle.
        /// </summary>
        public void Refresh()
        {
            if (CheckContentListValid())
            {
                Rows.Clear();
                GenerateHeaderRow();
                GenerateDataRows();
                GenerateButtonRow();
            }
        }

        /// <summary>
        /// Erzeugt die Kopfzeile.
        /// </summary>
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

        /// <summary>
        /// Erzeugt die Zeilen mit den Daten.
        /// </summary>
        private void GenerateDataRows()
        {
            int i = 0;
            foreach (Teilnehmer contentMember in Content)
            {
                i += 1;
                CustomRow newRow = new CustomRow(DisplayFields, contentMember);
                Rows.Add(newRow);
            }
        }
        private void GenerateButtonRow()
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            Button acceptButton = new Button()
            {
                Text = "accept",
                Visible = true,
                CausesValidation = true,
                ID = "btnAccept",
            };
            acceptButton.Click += acceptButton_Click;
            tc.Controls.Add(acceptButton);
            tr.Cells.Add(tc);
            Rows.Add(tr);
        }
        public void DeleteItem(CustomRow cr)
        {
            Content.RemoveAt(Rows.GetRowIndex(cr) - 1);
            Refresh();
        }

        public void ApplyRowChanges(int rowIndex)
        {
            (Content[rowIndex] as Person).Vorname = this.Parent.Parent.Parent.Page.Request.Form["ctl00$MainContent$txtBox0"];
            (Content[rowIndex] as Person).Nachname = this.Parent.Parent.Parent.Page.Request.Form["ctl00$MainContent$txtBox1"];
            (Content[rowIndex] as Person).Geburtstag = this.Parent.Parent.Parent.Page.Request.Form["ctl00$MainContent$txtBox2"];            
            Content[rowIndex].Speichern();
            Refresh();
        }

        public void SetRowInEditMode(CustomRow cr)
        {
            EditedRowIndex = Rows.GetRowIndex(cr);
            cr.RowState = RowState.rsEdit;
            cr.RefreshRow();
        }

        //Accept-Button OnClickEvent
        private void acceptButton_Click(object sender, EventArgs e)
        {
            ApplyRowChanges(EditedRowIndex);
        }
        #endregion
    }
}