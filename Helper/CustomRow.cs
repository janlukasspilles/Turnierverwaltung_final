using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung_final.View;

namespace Turnierverwaltung_final.Helper
{
    public class CustomRow : TableRow
    {
        #region Attributes
        private RowState _rowState;
        private Button _cancelButton;
        private Button _editButton;
        private Button _deleteButton;
        #endregion
        #region Properties
        public RowState RowState { get => _rowState; set => _rowState = value; }
        public Button EditButton { get => _editButton; set => _editButton = value; }
        public Button DeleteButton { get => _deleteButton; set => _deleteButton = value; }
        public Button CancelButton { get => _cancelButton; set => _cancelButton = value; }        
        #endregion
        #region Constructors
        #endregion
        public CustomRow(List<PropertyInfo> displayFields, Teilnehmer teilnehmer) : base()
        {
            TableCell newCell = null;
            foreach (PropertyInfo propertyInfo in displayFields)
            {
                newCell = new TableCell();
                Label l = new Label();
                l.Text = propertyInfo.PropertyType == typeof(bool) ? ((bool)propertyInfo.GetValue(teilnehmer)).GetJaNeinString() : propertyInfo.GetValue(teilnehmer).ToString();
                newCell.Controls.Add(l);
                Cells.Add(newCell);
            }

            RowState = RowState.rsNone;
            EditButton = new Button()
            {
                Text = "edit",
            };
            DeleteButton = new Button()
            {
                Text = "del",
            };            
            CancelButton = new Button()
            {
                Text = "cancel",
                Visible = false,
            };
            EditButton.Click += editButton_Click;
            DeleteButton.Click += delButton_Click;
            CancelButton.Click += cancelButton_Click;
            newCell = new TableCell();
            newCell.Controls.Add(EditButton);
            Cells.Add(newCell);
            newCell = new TableCell();
            newCell.Controls.Add(DeleteButton);
            Cells.Add(newCell);            
            newCell = new TableCell();
            newCell.Controls.Add(CancelButton);
            Cells.Add(newCell);
        }

        #region Methods
        public void RefreshRow()
        {
            switch (RowState)
            {
                case RowState.rsNone:
                    SetRowInNoneMode();
                    break;
                case RowState.rsEdit:
                    SetRowInEditMode();
                    break;
            }
        }
        private void SetRowInNoneMode()
        {
            for (int i = 0; i < Cells.Count - 4; i++)
            {
                foreach (System.Web.UI.Control co in Cells[i].Controls)
                {
                    if (co is TextBox)
                    {
                        string tmp = (co as TextBox).Text;
                        Cells[i].Controls.Remove(co);
                        Cells[i].Controls.Add(new Label() { Text = tmp });
                    }
                }
            }
            EditButton.Visible = true;
            DeleteButton.Visible = true;
            CancelButton.Visible = false;
        }
        private void SetRowInEditMode()
        {
            for (int i = 0; i < Cells.Count - 4; i++)
            {
                foreach (System.Web.UI.Control co in Cells[i].Controls)
                {
                    if (co is Label)
                    {
                        string tmp = (co as Label).Text;
                        Cells[i].Controls.Remove(co);
                        Cells[i].Controls.Add(new TextBox() { Text = tmp, ID = $"txtBox{i}"});
                    }
                }
            }
            EditButton.Visible = false;
            DeleteButton.Visible = true;
            CancelButton.Visible = true;
        }
        //Delete-Button OnClick Event
        private void delButton_Click(object sender, EventArgs e)
        {
            if (Parent is CustomTable)
                (Parent as CustomTable).DeleteItem(this);
        }

        //Edit-Button OnClickEvent
        private void editButton_Click(object sender, EventArgs e)
        {
            if (Parent is CustomTable)
            {
                (Parent as CustomTable).SetRowInEditMode(this);
            }
            //RowState = RowState.rsEdit;
            //RefreshRow();
        }        
        //Cancel-Button OnClickEvent
        private void cancelButton_Click(object sender, EventArgs e)
        {
            RowState = RowState.rsNone;
            RefreshRow();
        }
        #endregion
    }
    public enum RowState
    {
        rsNone,
        rsEdit
    }
}
