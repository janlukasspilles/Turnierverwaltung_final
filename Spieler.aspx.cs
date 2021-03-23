using System;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung_final.Helper;
using System.Linq;

namespace Turnierverwaltung_final.View
{
    public partial class Spieler : System.Web.UI.Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            //Viewhelpers.CreateGenericTable(Global.Controller.Teilnehmer).ForEach(x => tblFussballer.Rows.Add(x));            
            //pnl_tbl.Controls.Add(new CustomTable<Teilnehmer>(Controller.Teilnehmer));
            pnl_tbl.Controls.Add(new CustomTable<Teilnehmer>());
            (pnl_tbl.FindControl("tbl_custom") as CustomTable<Teilnehmer>).Content = Controller.Teilnehmer;
        }

        protected void ddl_selection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch ((sender as DropDownList).Text)
            //{
            //    case "Alle":
            //        //Controller.ZieheAlleSpieler();
            //        break;
            //    case "Fussballspieler":
            //        //Controller.ZieheFussballSpieler();
            //        break;
            //    case "Handballspieler":
            //        break;
            //    case "Tennisspieler":
            //        break;
            //}
            //(pnl_tbl.FindControl("tbl_custom") as CustomTable<Teilnehmer>).Content = Controller.Teilnehmer;            
        }
    }
}