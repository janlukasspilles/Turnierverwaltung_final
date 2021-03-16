using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace Turnierverwaltung_final.Helper
{
    public static class Viewhelpers
    {
        /// <summary>
        /// Erzeugt eine Tabelle zu einer Liste.
        /// Sofern die Objekte mit unterschiedlichem
        /// Typ in dieser Liste sind, werden diese 
        /// unter der ersten gemeinsamen Basisklasse 
        /// zusammengefasst.
        /// Lässt sich keine gemeinsame Basisklasse 
        /// finden, wird eine Exception geworfen.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<TableRow> CreateGenericTable<T>(List<T> content)
        {
            //Null
            if (content is null)
                throw new ArgumentNullException(nameof(content));
            //Liste leer
            if (!(content.Count > 0))
                return null;

            Type contentType = null;

            //Check, ob unterschiedliche Typen
            if (!content.All(x => x.GetType().Equals(content.First().GetType())))
            {
                //Checken, ob es eine gemeinsame Basisklasse gibt
                for (Type current = content.First().GetType(); current != null; current = current.BaseType)
                {
                    if (content.All(x => x.GetType().IsSubclassOf(current)))
                    {
                        contentType = current;
                        break;
                    }
                }
                //Keine gemeinsame Basisklasse
                if (contentType is null)
                    throw new Exception();
            }
            else
            {
                contentType = content.First().GetType();
            }

            List<TableRow> resultRows = new List<TableRow>();

            //Filtern nach DisplayFields
            PropertyInfo[] properties = FilterDisplayFields(contentType.GetProperties());

            //Headercells
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (PropertyInfo shownPropertyInfo in properties)
            {
                TableHeaderCell newHeaderCell = new TableHeaderCell();
                Button newBtn = new Button();
                newBtn.Text = shownPropertyInfo.Name;
                newBtn.Width = Unit.Percentage(90);
                newBtn.BorderStyle = BorderStyle.None;
                newBtn.ID = $"btn{shownPropertyInfo.Name}";
                newBtn.Style.Add("text-align", "left");
                newHeaderCell.Controls.Add(newBtn);
                headerRow.Cells.Add(newHeaderCell);
                resultRows.Add(headerRow);
            }

            //Datacells    
            foreach (T contentMember in content)
            {
                TableRow newRow = new TableRow();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    TableCell newCell = new TableCell();
                    newCell.Text = propertyInfo.GetValue(contentMember).ToString();
                    newRow.Cells.Add(newCell);
                }
                resultRows.Add(newRow);
            }
            return resultRows;
        }

        /// <summary>
        /// Erzeugt eine Tabelle zu einer Selektionsmenge.
        /// Soll für Views genutzt werden, falls Daten nicht 
        /// bearbeitet werden müssen.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<TableRow> CreateGenericTable(MySqlDataReader content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            //Selektionsergebnis leer
            if (!(content.FieldCount > 0))
                return null;

            List<TableRow> resultRows = new List<TableRow>();
            string[] columns = new string[content.FieldCount];
            for (int zaehler = 0; zaehler < content.FieldCount; zaehler++)
            {
                columns[zaehler] = content.GetName(zaehler);
            }

            //Headercells
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (string columnName in columns)
            {
                TableHeaderCell newHeaderCell = new TableHeaderCell();
                Button newBtn = new Button();
                newBtn.Text = columnName;
                newBtn.Width = Unit.Percentage(90);
                newBtn.BorderStyle = BorderStyle.None;
                newBtn.ID = $"btn{columnName}";
                newBtn.Style.Add("text-align", "left");
                newHeaderCell.Controls.Add(newBtn);
                headerRow.Cells.Add(newHeaderCell);
                resultRows.Add(headerRow);
            }

            //Datacells    
            while (content.Read())
            {
                TableRow newRow = new TableRow();
                foreach (string columnName in columns)
                {
                    TableCell newCell = new TableCell();
                    newCell.Text = content.GetValue(content.GetOrdinal(columnName)).ToString();
                    newRow.Cells.Add(newCell);
                }
                resultRows.Add(newRow);
            }

            return resultRows;
        }


        //Filtert nach Feldern, die DisplayNameAttribute gesetzt haben
        private static PropertyInfo[] FilterDisplayFields(PropertyInfo[] p)
        {
            List<PropertyInfo> res = p.ToList();
            foreach (PropertyInfo propertyInfo in p)
            {
                if (!Attribute.IsDefined(propertyInfo, typeof(DisplayNameAttribute)))
                    res.Remove(propertyInfo);
            }
            return res.ToArray();
        }
    }

}