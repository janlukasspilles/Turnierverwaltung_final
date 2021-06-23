using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.Model.TeilnehmerNS;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung_final.View
{
    public partial class Einstellungen : Page
    {
        private Controller _controller;
        private CustomTable<Teilnehmer> curTable;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            curTable = new CustomTable<Teilnehmer>(Type.GetType($"Turnierverwaltung.Model.TeilnehmerNS.Mannschaft"));
            curTable.OnHeaderButton_ClickCommand += OnHeaderButton_Click;
            curTable.DeleteButton_ClickCommand += OnDeleteButton_Click;
            curTable.AddButton_ClickCommand += OnAddButton_Click;
            curTable.SubmitButton_ClickCommand += OnSubmitButton_Click;
            if (!IsPostBack)
            {
                Controller.GetAlleMannschaften();
            }
            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();

            pnl_tbl.Controls.Add(curTable);
        }

        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Mannschaft m = new Mannschaft();
            Controller.Teilnehmer.Add(m);

            curTable.DataSource = Controller.Teilnehmer;
            curTable.DataBind();
        }

        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            var tmp = Controller.Teilnehmer.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            if (Controller.Teilnehmer.SequenceEqual(tmp))
                Controller.Teilnehmer = Controller.Teilnehmer.OrderByDescending(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            else
                Controller.Teilnehmer = tmp;

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
                                ListDataType.GetProperty(curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType((curTable.Rows[row + 1].Cells[i].Controls[0] as Label).Text, curTable.DisplayFields[i].PropertyType));
                                break;
                            case ControlType.ctDomain:
                                int domainId = (curTable.Rows[row + 1].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                                ListDataType.GetProperty(curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(domainId, curTable.DisplayFields[i].PropertyType));
                                break;
                        }
                    }
                }
                if (Controller.Teilnehmer[row - 1].Id == 0)
                {
                    Controller.Teilnehmer[row - 1].Neuanlage();
                }
                else
                {
                    Controller.Teilnehmer[row - 1].Speichern();
                }
            }
            curTable.DataSource = Controller.Teilnehmer;
            //curTable.DataBind();
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
        protected void Page_PreInit(object sender, EventArgs e)
        {
        }
        protected void Page_Init(object sender, EventArgs e)
        {
        }
        protected void Page_InitComplete(object sender, EventArgs e)
        {
        }
        protected override void OnPreLoad(EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
        }
        protected override void OnPreRender(EventArgs e)
        {
        }
        protected override void OnSaveStateComplete(EventArgs e)
        {
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {
        }
    }


}