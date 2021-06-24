using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung_final.Model;

namespace Turnierverwaltung_final.Helper
{
    public class CustomTable<T> : Table
    {
        #region Attributes
        private List<T> _dataSource;
        private Type _listDataType;
        private string[] _ignoreFields;
        private List<PropertyInfo> _displayFields;
        private CommandEventHandler _onHeaderButton_ClickCommand;
        private EventHandler _deleteButton_ClickCommand;
        private EventHandler _addButton_ClickCommand;
        private EventHandler _submitButton_ClickCommand;
        private EventHandler _cancelButton_ClickCommand;
        private Dictionary<string, IList> _domainDictionary;
        private Type _fallbackType;
        #endregion
        #region Properties
        public List<T> DataSource { get => _dataSource; set => _dataSource = value; }
        public Type ListDataType { get => _listDataType; set => _listDataType = value; }
        public string[] IgnoreFields { get => _ignoreFields; set => _ignoreFields = value; }
        public List<PropertyInfo> DisplayFields { get => _displayFields; set => _displayFields = value; }
        public CommandEventHandler OnHeaderButton_ClickCommand { get => _onHeaderButton_ClickCommand; set => _onHeaderButton_ClickCommand = value; }
        public Dictionary<string, IList> DomainDictionary { get => _domainDictionary; set => _domainDictionary = value; }
        public List<int> RowsInEdit
        {
            get
            {
                return (List<int>)ViewState["RowsInEdit"];
            }
            set => ViewState["RowsInEdit"] = value;
        }
        public EventHandler DeleteButton_ClickCommand { get => _deleteButton_ClickCommand; set => _deleteButton_ClickCommand = value; }
        public Type FallbackType { get => _fallbackType; set => _fallbackType = value; }
        public EventHandler AddButton_ClickCommand { get => _addButton_ClickCommand; set => _addButton_ClickCommand = value; }
        public EventHandler SubmitButton_ClickCommand { get => _submitButton_ClickCommand; set => _submitButton_ClickCommand = value; }
        public EventHandler CancelButton_ClickCommand { get => _cancelButton_ClickCommand; set => _cancelButton_ClickCommand = value; }
        #endregion
        #region Constructors        
        public CustomTable() : base()
        {
            IgnoreFields = new string[] { };
            DisplayFields = new List<PropertyInfo>();
            DomainDictionary = new Dictionary<string, IList>();
            List<Sportart> tmp = new List<Sportart>();
            Sportart sp = new Sportart();
            sp.SelektionId(1);
            tmp.Add(sp);
            sp = new Sportart();
            sp.SelektionId(2);
            tmp.Add(sp);
            sp = new Sportart();
            sp.SelektionId(3);
            tmp.Add(sp);
            DomainDictionary.Add("SPORTART", tmp.ToList());
            CssClass = "table table-bordered";
        }
        public CustomTable(Type fallbackType) : base()
        {
            IgnoreFields = new string[] { };
            DisplayFields = new List<PropertyInfo>();
            DomainDictionary = new Dictionary<string, IList>();
            List<Sportart> tmp = new List<Sportart>();
            Sportart sp = new Sportart();
            sp.SelektionId(1);
            tmp.Add(sp);
            sp = new Sportart();
            sp.SelektionId(2);
            tmp.Add(sp);
            sp = new Sportart();
            sp.SelektionId(3);
            tmp.Add(sp);
            DomainDictionary.Add("SPORTART", tmp.ToList());
            CssClass = "table table-bordered";
            FallbackType = fallbackType;
        }
        #endregion
        #region Methods
        public override void DataBind()
        {

            SetListDatatype();
            SetDisplayFields();
            BuildTable();

        }

        private void BuildTable()
        {
            Rows.Clear();
            SetHeaderRow();
            SetDataRows();
            SetFooterRow();
        }
        private void SetDataRows()
        {
            for (int i = 0; i < DataSource.Count; i++)
            {
                if (RowsInEdit != null && RowsInEdit.Contains(i))
                    SetDataRowNew(DataSource[i], i + 1, true);
                else
                    SetDataRowNew(DataSource[i], i + 1, false);
            }
        }
        private void SetListDatatype()
        {
            if (FallbackType != null)
                ListDataType = FallbackType;
            else
            {
                Type t = DataSource.First().GetType();
                if (!DataSource.All(x => x.GetType().Equals(t)))
                {
                    //Checken, ob es eine gemeinsame Basisklasse gibt
                    for (Type current = t; current != null; current = current.BaseType)
                    {
                        if (DataSource.All(x => x.GetType().IsSubclassOf(current)))
                        {
                            ListDataType = current;
                        }
                    }
                    //Keine gemeinsame Basisklasse
                    throw new Exception("Keine gemeinsame Basisklasse!");
                }
                else
                {
                    ListDataType = t;
                }
            }
        }

        private void SetDisplayFields()
        {
            DisplayFields.Clear();
            if (ListDataType != null)
            {
                foreach (PropertyInfo propertyInfo in ListDataType.GetProperties())
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(DisplayMetaInformation)) && !IgnoreFields.Any(propertyInfo.Name.Contains))
                        DisplayFields.Add(propertyInfo);
                }
                DisplayFields = DisplayFields.OrderBy(p => (p.GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Order).ToList();
            }
        }

        private void SetHeaderRow()
        {
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (PropertyInfo shownPropertyInfo in DisplayFields)
            {
                TableHeaderCell newHeaderCell = new TableHeaderCell();
                Button newBtn = new Button
                {
                    Text = (shownPropertyInfo.GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Displayname,
                    Width = Unit.Percentage(100),
                    BorderStyle = BorderStyle.None,
                    ID = $"btn{shownPropertyInfo.Name}"
                };
                newBtn.Style.Add("text-align", "left");
                newBtn.CommandArgument = shownPropertyInfo.Name;
                newBtn.Command += OnHeaderButton_ClickCommand;
                newHeaderCell.Controls.Add(newBtn);
                headerRow.Cells.Add(newHeaderCell);
            }
            Rows.Add(headerRow);
        }

        private void SetDataRowNew(T member, int pos, bool editable)
        {
            TableRow tr = new TableRow();
            TableCell newCell;
            for (int counter = 0; counter < DisplayFields.Count; counter++)
            {
                newCell = new TableCell() { ID = $"tblCell{counter}Row{pos}" };
                Control newControl = null;
                DisplayMetaInformation dmi = DisplayFields[counter].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;

                string controlId = $"con{counter}Row{pos}";
                var curValueOfProperty = member.GetType().GetProperty(DisplayFields[counter].Name).GetValue(member, null);

                switch (dmi.ControlType)
                {
                    case ControlType.ctEditText:

                        if ((editable && dmi.Editable) || (Context.Request.Form.AllKeys.Contains("ctl00$MainContent$" + controlId) && Context.Request.Form["ctl00$MainContent$" + controlId].ToString() != curValueOfProperty.ToString()))
                        {
                            newControl = new TextBox() { ID = controlId, CssClass = "form-control" };
                            (newControl as TextBox).Text = curValueOfProperty?.ToString() ?? "";
                        }
                        else
                        {
                            newControl = new Label() { ID = controlId };
                            (newControl as Label).Text = curValueOfProperty?.ToString() ?? "";
                        }
                        break;
                    case ControlType.ctDomain:
                        if (DomainDictionary.TryGetValue(dmi.DomainName, out IList domainList))
                        {
                            if (editable && dmi.Editable || (Context.Request.Form.AllKeys.Contains("ctl00$MainContent$" + controlId) && Context.Request.Form["ctl00$MainContent$" + controlId].ToString() != domainList[Convert.ToInt32(curValueOfProperty) - 1].ToString()))
                            {
                                newControl = new DropDownList { ID = controlId, CssClass = "form-control" };
                                (newControl as DropDownList).DataSource = domainList;
                                (newControl as DropDownList).DataBind();
                                (newControl as DropDownList).SelectedIndex = Convert.ToInt32(curValueOfProperty?.ToString()) - 1;
                            }
                            else
                            {
                                newControl = new Label() { ID = controlId };
                                (newControl as Label).Text = domainList[Convert.ToInt32(curValueOfProperty?.ToString()) - 1].ToString();
                            }
                        }
                        else
                        {
                            throw new Exception("Keine Domainliste hinterlegt");
                        }
                        break;
                    case ControlType.ctCheck:
                        //if ((!editable || dmi.Editable) && !Context.Request.Form.AllKeys.Contains("ctl00$MainContent$" + controlId))
                        //{
                        //    newControl = new CheckBox() { ID = controlId };
                        //    (newControl as CheckBox).Checked = (bool)valueTmp;
                        //    (newControl as CheckBox).Enabled = false;
                        //}
                        break;
                    default:
                        throw new Exception("Fehler!!");
                }
                newCell.Controls.Add(newControl);
                tr.Cells.Add(newCell);
            }
            if (!editable)
            {
                newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
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

                newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                CheckBox SelectedCheckBox = new CheckBox() { ID = $"cbSelected{pos}" };
                newCell.Controls.Add(SelectedCheckBox);
                tr.Cells.Add(newCell);
            }
            Rows.Add(tr);
        }

        private void OnEditButton_Click(object sender, CommandEventArgs e)
        {
            if (RowsInEdit is null)
                RowsInEdit = new List<int>();
            RowsInEdit.Add(Rows.GetRowIndex((sender as Control).Parent.Parent as TableRow) - 1);
            DataBind();
        }

        private void SetFooterRow()
        {
            TableFooterRow tr = new TableFooterRow();

            TableCell tc = new TableCell();
            //Delete Button
            Button DeleteButton = new Button()
            {
                Text = "Ausgewählte löschen",
                Visible = true,
                ID = "btnDelete",
                CssClass = "btn btn-secondary",
            };
            DeleteButton.Click += DeleteButton_ClickCommand;

            tc.Controls.Add(DeleteButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Add Button
            Button AddButton = new Button()
            {
                Text = "Hinzufügen",
                Visible = true,
                ID = "btnAddMannschaft",
                CssClass = "btn btn-secondary",
            };
            AddButton.Click += AddButton_ClickCommand;
            AddButton.Click += OnAddButton_Click;
            tc.Controls.Add(AddButton);
            tr.Cells.Add(tc);


            tc = new TableCell();
            //Submit Button
            Button SubmitButton = new Button()
            {
                Text = "Änderungen speichern",
                Visible = true,
                Enabled = RowsInEdit != null && RowsInEdit.Count != 0,
                CssClass = "btn btn-secondary",
                ID = "btnAccept",
            };
            SubmitButton.Click += SubmitButton_ClickCommand;
            SubmitButton.Click += OnSubmitButton_Click;
            tc.Controls.Add(SubmitButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Cancel Button
            Button CancelButton = new Button()
            {
                Text = "Änderungen verwerfen",
                Visible = true,
                Enabled = RowsInEdit != null && RowsInEdit.Count != 0,
                CssClass = "btn btn-secondary",
                ID = "btnCancel",
            };
            CancelButton.Click += CancelButton_ClickCommand;
            CancelButton.Click += OnCancelButton_Click;
            tc.Controls.Add(CancelButton);
            tr.Cells.Add(tc);

            Rows.Add(tr);
        }

        private void OnCancelButton_Click(object sender, EventArgs e)
        {
            if (RowsInEdit != null)
                RowsInEdit.Clear();
            DataBind();
        }

        private void OnAddButton_Click(object sender, EventArgs e)
        {
            if (RowsInEdit is null)
                RowsInEdit = new List<int>();
            RowsInEdit.Add(DataSource.Count - 1);
            DataBind();
        }

        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            if (RowsInEdit != null)
                RowsInEdit.Clear();
            DataBind();
        }
        #endregion
    }
}
