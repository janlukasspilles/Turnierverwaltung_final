using System;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung_final.Helper;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Reflection;
using Turnierverwaltung.Model.TeilnehmerNS;

namespace Turnierverwaltung_final.View
{
    public partial class Personen : Page
    {
        #region Attributes
        private Controller _controller;
        private CustomTable<Teilnehmer> curTable;
        private readonly string[] _ignoreFields = { "Name" };
        public Type ShownType
        {
            get
            {
                if (ViewState["ShownType"] != null)
                    return (Type)ViewState["ShownType"];
                return null;
            }
            set => ViewState["ShownType"] = value;
        }
        #endregion
        #region Properties
        public Controller Controller { get => _controller; set => _controller = value; }

        #endregion
        #region Constructors
        public Personen() : base()
        {

        }
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            if (!IsPostBack)
            {
                Controller.Teilnehmer.Clear();
            }
            if (ShownType != null)
            {
                (this.FindControlRecursive("btn" + ShownType.Name) as Button).CssClass = "btn btn-warning";
                BuildTable();
            }
        }
        private void ResetButtons()
        {
            btnTrainer.CssClass = "btn btn-dark";
            btnPhysio.CssClass = "btn btn-dark";
            btnFussballspieler.CssClass = "btn btn-dark";
            btnHandballspieler.CssClass = "btn btn-dark";
            btnTennisspieler.CssClass = "btn btn-dark";
        }

        private void BuildTable()
        {
            if (curTable == null)
            {
                curTable = new CustomTable<Teilnehmer>(ShownType);
                curTable.OnHeaderButton_ClickCommand += OnHeaderButton_Click;
                curTable.DeleteButton_ClickCommand += OnDeleteButton_Click;
                curTable.AddButton_ClickCommand += OnAddButton_Click;
                curTable.SubmitButton_ClickCommand += OnSubmitButton_Click;
                curTable.IgnoreFields = _ignoreFields;
            }
            curTable.FallbackType = ShownType;
            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();

            pnl_tbl.Controls.Add(curTable);
        }

        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {            
            List<Teilnehmer> ListeSortiertAsc = Controller.Teilnehmer.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            if (Controller.Teilnehmer.SequenceEqual(ListeSortiertAsc))
            {
                Controller.Teilnehmer = Controller.Teilnehmer.OrderByDescending(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
                curTable.Sorted = Tuple.Create(curTable.FallbackType.GetProperty(e.CommandArgument.ToString()), SortDirection.Descending);
            }
            else
            {
                Controller.Teilnehmer = ListeSortiertAsc;
                curTable.Sorted = Tuple.Create(curTable.FallbackType.GetProperty(e.CommandArgument.ToString()), SortDirection.Ascending);
            }

            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();
        }

        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            Queue<Teilnehmer> deletequeue = new Queue<Teilnehmer>();
            foreach (TableRow tmp in curTable.Rows)
            {
                if (!(tmp is TableHeaderRow) && !(tmp is TableFooterRow))
                {
                    if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{curTable.Rows.GetRowIndex(tmp)}") is CheckBox cb)
                    {
                        if (cb.Checked)
                        {
                            Teilnehmer t = Controller.Teilnehmer[curTable.Rows.GetRowIndex(tmp) - 1];
                            t.Loeschen();
                            deletequeue.Enqueue(t);
                        }
                    }
                }
            }
            foreach (Teilnehmer t in deletequeue)
            {
                Controller.Teilnehmer.Remove(t);
            }
            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();
        }

        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Teilnehmer t = (Teilnehmer)Activator.CreateInstance(curTable.FallbackType);
            Controller.Teilnehmer.Add(t);

            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();
        }

        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            Type ListDataType = curTable.FallbackType ?? curTable.ListDataType;
            foreach (int row in curTable.RowsInEdit)
            {
                for (int i = 0; i < curTable.DisplayFields.Count; i++)
                {
                    DisplayMetaInformation dmi = curTable.DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                    if (dmi.Editable)
                    {
                        switch (dmi.ControlType)
                        {
                            case ControlType.ctEditText:
                                if (curTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (curTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(value, curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDomain:
                                if (curTable.Rows[row + 1].Cells[i].Controls[0] is DropDownList)
                                {
                                    int domainId = (curTable.Rows[row + 1].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                                    ListDataType.GetProperty(curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(domainId, curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDate:
                                if (curTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (curTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(value, curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                        }
                    }
                }
                if (Controller.Teilnehmer[row].Id == 0)
                {
                    Controller.Teilnehmer[row].Neuanlage();
                }
                else
                {
                    Controller.Teilnehmer[row].Speichern();
                }
            }
            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();
        }
        protected void OnTypeSelected(object sender, EventArgs e)
        {
            Controller.Teilnehmer.Clear();
            switch ((sender as Button).CommandArgument)
            {
                case "Trainer":
                    Controller.GetAlleTrainer();
                    break;
                case "Physio":
                    Controller.GetAllePhysios();
                    break;
                case "Fussballspieler":
                    Controller.GetAlleFussballspieler();
                    break;
                case "Handballspieler":
                    Controller.GetAlleHandballspieler();
                    break;
                case "Tennisspieler":
                    Controller.GetAlleTennisspieler();
                    break;
            }
            ShownType = Type.GetType($"Turnierverwaltung_final.Model.TeilnehmerNS.Personen.{(sender as Button).CommandArgument}");
            ResetButtons();
            (sender as Button).CssClass = "btn btn-warning";
            BuildTable();
        }
    }
    #endregion
}
