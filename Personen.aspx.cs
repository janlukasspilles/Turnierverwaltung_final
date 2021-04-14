using System;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung_final.Helper;
using System.Linq;

namespace Turnierverwaltung_final.View
{
    public partial class Personen : System.Web.UI.Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            if (!IsPostBack)
            {
                //Do things only when page loads for the first time. 
            }
            LoadTable();
        }

        private void LoadTable()
        {
            //pnl_tbl.TemplateControl.Controls.Add(new CustomTable());
            pnl_tbl.Controls.Add(new CustomTable());
            //(pnl_tbl.TemplateControl.FindControl("tbl_custom") as CustomTable).Content = Controller.Teilnehmer;
            (pnl_tbl.FindControl("tbl_custom") as CustomTable).Content = Controller.Teilnehmer;
            //ViewState["CurrentTable"] = pnl_tbl.FindControl("tbl_custom") as CustomTable;
        }
    }
}