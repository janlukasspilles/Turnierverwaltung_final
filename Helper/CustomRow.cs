using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung_final.View;

namespace Turnierverwaltung_final.Helper
{
    public class CustomRow : TableRow
    {
        #region Attributes        
        private Button _editButton;
        private CheckBox _selectedCheckBox;
        private bool _inEditMode;
        #endregion
        #region Properties
        public Button EditButton { get => _editButton; set => _editButton = value; }
        public CheckBox SelectedCheckBox { get => _selectedCheckBox; set => _selectedCheckBox = value; }
        public bool InEditMode
        {
            get
            {
                if (this.ViewState["InEditMode"] == null)
                    return _inEditMode;
                return (bool)this.ViewState["InEditMode"];
            }
            set
            {
                this.ViewState["InEditMode"] = value;
                _inEditMode = value;
            }
        }
        #endregion
        #region Constructors
        #endregion
        public CustomRow(Teilnehmer contentMember, int pos, List<PropertyInfo> displayFields) : base()
        {
            ID = $"tblRow{pos}";
            if (contentMember != null && displayFields != null)
            {
                for (int counter = 0; counter < displayFields.Count; counter++)
                {
                    TableCell newCell = new TableCell() { ID = $"tblCell{counter}Row{pos}" };
                    Control newControl = null;
                    if (!InEditMode)
                    {
                        newControl = new Label() { ID = $"lbl{counter}Row{pos}" };
                        (newControl as Label).Text = contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null).ToString();
                    }
                    else
                    {
                        newControl = new TextBox() { ID = $"txt{counter}Row{pos}" };
                        (newControl as TextBox).Text = contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null).ToString();
                    }
                    newCell.Controls.Add(newControl);
                    Cells.Add(newCell);
                }
                AddUserControls(pos);
            }
        }

        private void AddUserControls(int pos)
        {
            TableCell newCell = new TableCell() { ID = $"tblCell{Cells.Count}Row{pos}" };
            //Edit-Button
            EditButton = new Button();
            EditButton.ID = $"btnEdit{pos}";
            EditButton.Text = "Edit";
            EditButton.Click += editButton_Click;
            newCell.Controls.Add(EditButton);
            Cells.Add(newCell);
            //CheckBox
            newCell = new TableCell() { ID = $"tblCell{Cells.Count}Row{pos}" };
            SelectedCheckBox = new CheckBox() { ID = $"cbSelected{pos}" };
            newCell.Controls.Add(SelectedCheckBox);
            Cells.Add(newCell);
        }

        //Convert.ChangeType(contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null), displayFields[counter].PropertyType);
        #region Methods
        public void RefreshRow()
        {
            if (InEditMode)
                SetRowInEditMode();
            else
                SetRowInNoneMode();
        }
        private void SetRowInNoneMode()
        {

        }
        private void SetRowInEditMode()
        {
            //-2, da die letzten zwei Spalten Checkbox und Button sind
            for (int counter = 0; counter < Cells.Count - 2; counter++)
            {
                foreach (Control c in Cells[counter].Controls)
                {
                    if (c is Label)
                    {
                        TextBox txtTmp = new TextBox()
                        {
                            ID = (c as Label).ID.Replace("lbl", "txt"),
                            Text = (c as Label).Text,
                        };
                        Cells[counter].Controls.Remove(c);
                        Cells[counter].Controls.Add(txtTmp);
                    }
                }
            }
        }

        //Edit-Button OnClickEvent
        private void editButton_Click(object sender, EventArgs e)
        {
            InEditMode = true;
            RefreshRow();
        }
        #endregion
    }
}
