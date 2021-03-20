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
using System.Web.UI.WebControls;

namespace Turnierverwaltung_final.Helper
{
    public class CustomTable<T> : Table
    {
        #region Attributes
        private List<T> _content;
        private List<PropertyInfo> _displayFields;
        private Type _listDataType;
        #endregion
        #region Properties
        public List<T> Content
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
        #endregion
        #region Constructors
        public CustomTable()
        {
            DisplayFields = new List<PropertyInfo>();
            Content = new List<T>();
            ID = "tbl_custom";
        }
        public CustomTable(List<T> content)
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
            foreach (T contentMember in Content)
            {
                TableRow newRow = new TableRow();
                foreach (PropertyInfo propertyInfo in DisplayFields)
                {
                    TableCell newCell = new TableCell();
                    newCell.Text = propertyInfo.GetValue(contentMember).ToString();
                    newRow.Cells.Add(newCell);
                }
                Rows.Add(newRow);
            }
        }
        #endregion
    }
}