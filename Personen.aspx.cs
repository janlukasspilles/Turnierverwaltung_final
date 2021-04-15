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
            Controller.GetAllePersonen();
            LoadTable();
        }

        private void LoadTable()
        {
            pnl_tbl.Controls.Add(new CustomTable(Controller.Teilnehmer));
        }
    }
}