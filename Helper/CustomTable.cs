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
        private List<Tuple<string, string, string, Action<object, CommandEventArgs>>> _additionalRowButtons;
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
        public List<Tuple<string, string, string, Action<object, CommandEventArgs>>> AdditionalRowButtons { get => _additionalRowButtons; set => _additionalRowButtons = value; }
        public Tuple<PropertyInfo, SortDirection> Sorted
        {
            get
            {
                if (ViewState["Sorted"] != null)
                {
                    return (Tuple<PropertyInfo, SortDirection>)ViewState["Sorted"];
                }
                return null;
            }
            set => ViewState["Sorted"] = value;
        }

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
                    SetDataRow(DataSource[i], i + 1, true);
                else
                    SetDataRow(DataSource[i], i + 1, false);
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
                LinkButton newBtn = new LinkButton
                {
                    Text = (shownPropertyInfo.GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation).Displayname,
                    ID = $"btn{shownPropertyInfo.Name}",
                    CssClass = "btn btn-primary",
                    Width = Unit.Percentage(100),
                };
                if(Sorted != null && Sorted.Item1.Equals(shownPropertyInfo))
                {
                    if (Sorted.Item2 == SortDirection.Ascending)
                        newBtn.CssClass = "btn btn-primary glyphicon glyphicon-chevron-down";
                    else
                        newBtn.CssClass = "btn btn-primary glyphicon glyphicon-chevron-up";
                }
                newBtn.Style.Add("text-align", "left");
                newBtn.Style.Add("display", "block");
                newBtn.Style.Add("margin", "auto");
                newBtn.CommandArgument = shownPropertyInfo.Name;
                newBtn.Command += OnHeaderButton_ClickCommand;
                newHeaderCell.Controls.Add(newBtn);
                headerRow.Cells.Add(newHeaderCell);
            }
            Rows.Add(headerRow);
        }

        private void SetDataRow(T member, int pos, bool editable)
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
                        if ((!editable || dmi.Editable) && !Context.Request.Form.AllKeys.Contains("ctl00$MainContent$" + controlId))
                        {
                            newControl = new CheckBox() { ID = controlId };
                            (newControl as CheckBox).Checked = Convert.ToBoolean(curValueOfProperty?.ToString());
                            (newControl as CheckBox).Enabled = true;
                        }
                        else
                        {
                            newControl = new CheckBox() { ID = controlId };
                            (newControl as CheckBox).Checked = Convert.ToBoolean(curValueOfProperty?.ToString());
                            (newControl as CheckBox).Enabled = false;
                        }
                        break;
                    case ControlType.ctDate:
                        if ((editable && dmi.Editable) || (Context.Request.Form.AllKeys.Contains("ctl00$MainContent$" + controlId) && Context.Request.Form["ctl00$MainContent$" + controlId].ToString() != curValueOfProperty.ToString()))
                        {
                            newControl = new TextBox() { ID = controlId, TextMode = TextBoxMode.Date, CssClass = "form-control" };
                            (newControl as TextBox).Text = curValueOfProperty?.ToString() ?? "";
                        }
                        else
                        {
                            newControl = new Label() { ID = controlId };
                            (newControl as Label).Text = curValueOfProperty?.ToString() ?? "";
                        }
                        break;
                    default:
                        throw new Exception("Fehler!!");
                }
                newCell.Controls.Add(newControl);
                tr.Cells.Add(newCell);
            }

            //Additional Row Buttons
            if (AdditionalRowButtons != null && AdditionalRowButtons.Count > 0)
            {
                foreach (var tmp in AdditionalRowButtons)
                {
                    newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                    Button b = new Button()
                    {
                        ID = tmp.Item1 + $"{pos}",
                        Text = tmp.Item2,
                        CssClass = tmp.Item3
                    };
                    b.Command += (o, e) => tmp.Item4(o, e);
                    b.CommandArgument = pos.ToString();
                    newCell.Controls.Add(b);
                    tr.Cells.Add(newCell);
                }
            }

            if (!editable)
            {
                newCell = new TableCell() { ID = $"tblCell{tr.Cells.Count}Row{pos}" };
                //Edit-Button
                LinkButton EditButton = new LinkButton
                {
                    ID = $"btnEdit{pos}",
                    //Text = "Editieren",
                    //Width = Unit.Percentage(100),                    
                    CssClass = "btn btn-primary glyphicon glyphicon-pencil"
                };
                EditButton.Command += OnEditButton_Click;

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
            LinkButton DeleteButton = new LinkButton()
            {
                Text = "Löschen",
                Visible = true,
                ID = "btnDelete",
                CssClass = "btn btn-danger glyphicon glyphicon-trash",
                //Width = Unit.Percentage(100),
            };
            DeleteButton.Click += DeleteButton_ClickCommand;

            tc.Controls.Add(DeleteButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Add Button
            LinkButton AddButton = new LinkButton()
            {
                Text = "Hinzufügen",
                Visible = true,
                ID = "btnAddMannschaft",
                CssClass = "btn btn-warning glyphicon glyphicon-plus",
                //Width = Unit.Percentage(100),
            };
            AddButton.Click += AddButton_ClickCommand;
            AddButton.Click += OnAddButton_Click;
            tc.Controls.Add(AddButton);
            tr.Cells.Add(tc);


            tc = new TableCell();
            //Submit Button
            LinkButton SubmitButton = new LinkButton()
            {
                Text = "Speichern",
                Visible = true,
                Enabled = RowsInEdit != null && RowsInEdit.Count != 0,
                CssClass = "btn btn-success glyphicon glyphicon-ok",
                ID = "btnAccept",
                //Width = Unit.Percentage(100),
            };
            SubmitButton.Click += SubmitButton_ClickCommand;
            SubmitButton.Click += OnSubmitButton_Click;
            tc.Controls.Add(SubmitButton);
            tr.Cells.Add(tc);

            tc = new TableCell();
            //Cancel Button
            LinkButton CancelButton = new LinkButton()
            {
                Text = "Verwerfen",
                Visible = true,
                //Enabled = RowsInEdit != null && RowsInEdit.Count != 0,
                CssClass = "btn btn-danger glyphicon glyphicon-remove",
                ID = "btnCancel",
                //Width = Unit.Percentage(100),
            };
            CancelButton.Attributes.Add("onClick", "return !this.disabled;");
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
