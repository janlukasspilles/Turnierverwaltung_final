using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;
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
        public RowState RowState
        {
            get
            {
                if (ViewState["RowState"] == null)
                    return RowState.rsNone;
                return (RowState)ViewState["RowState"];
            }
            set
            {
                ViewState["RowState"] = value;
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
                    if (RowState == RowState.rsNone)
                    {
                        newControl = new Label() { ID = $"lbl{counter}Row{pos}" };
                        (newControl as Label).Text = contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null).ToString();
                    }
                    else if(RowState == RowState.rsEdit)
                    {
                        newControl = new TextBox() { ID = $"txt{counter}Row{pos}", CssClass = "form-control" };
                        (newControl as TextBox).Text = contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null).ToString();
                    }
                    else if (RowState == RowState.rsInsert)
                    {
                        newControl = new TextBox() { ID = $"txt{counter}Row{pos}", CssClass = "form-control" };
                    }
                    newCell.Controls.Add(newControl);
                    Cells.Add(newCell);
                }
                AddUserControls(pos);
            }
        }

        public CustomRow(Teilnehmer contentMember, int pos, List<PropertyInfo> displayFields, RowState rs) : base()
        {
            RowState = rs;
            ID = $"tblRow{pos}";
            if (contentMember != null && displayFields != null)
            {
                for (int counter = 0; counter < displayFields.Count; counter++)
                {
                    TableCell newCell = new TableCell() { ID = $"tblCell{counter}Row{pos}" };
                    Control newControl = null;
                    if (RowState == RowState.rsNone)
                    {
                        newControl = new Label() { ID = $"lbl{counter}Row{pos}" };
                        (newControl as Label).Text = contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null).ToString();
                    }
                    else if (RowState == RowState.rsEdit)
                    {
                        newControl = new TextBox() { ID = $"txt{counter}Row{pos}", CssClass = "form-control" };
                        (newControl as TextBox).Text = contentMember.GetType().GetProperty(displayFields[counter].Name).GetValue(contentMember, null).ToString();
                    }
                    else if(RowState == RowState.rsInsert)
                    {
                        newControl = new TextBox() { ID = $"txt{counter}Row{pos}", CssClass = "form-control" };
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
            EditButton.CssClass = "btn btn-success";
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
            if (RowState == RowState.rsEdit)
                SetRowInEditMode();
            else if(RowState == RowState.rsNone)
                SetRowInNoneMode();
        }

        private void SetRowInInsertMode()
        {
            
        }
        private void SetRowInNoneMode()
        {
            //-2, da die letzten zwei Spalten Checkbox und Button sind
            for (int counter = 0; counter < Cells.Count - 2; counter++)
            {
                foreach (Control c in Cells[counter].Controls)
                {
                    if (c is TextBox)
                    {
                        Label lblTmp = new Label()
                        {
                            ID = (c as TextBox).ID.Replace("txt", "lbl"),
                            Text = (c as TextBox).Text,
                        };
                        Cells[counter].Controls.Remove(c);
                        Cells[counter].Controls.Add(lblTmp);
                    }
                }
            }
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
                            CssClass = "form-control",
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
            RowState = RowState.rsEdit;
            RefreshRow();
        }
        #endregion
    }
}
