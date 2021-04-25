using System;
using System.Web.UI.WebControls;
using Turnierverwaltung;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung_final.Helper;
using System.Linq;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;
using System.Web.UI;

namespace Turnierverwaltung_final.View
{
    public partial class Personen : Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;

            switch (ddl_selection.SelectedValue)
            {
                case "Alle":
                    Controller.GetAllePersonen();
                    break;
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
            LoadTable();

            if (IsPostBack)
            {
                if((pnl_tbl.FindControl("tbl_custom") as CustomTable).HasNewEntry)
                {
                    (pnl_tbl.FindControl("tbl_custom") as CustomTable).GenerateNewEntryRow();
                }
                foreach (TableRow r in (pnl_tbl.FindControl("tbl_custom") as CustomTable).Rows)
                {
                    if (r is CustomRow)
                        if ((r as CustomRow).RowState == RowState.rsEdit)
                            (r as CustomRow).RefreshRow();
                }
            }
        }

        private void LoadTable()
        {
            pnl_tbl.Controls.Add(new CustomTable(Controller.Teilnehmer));
        }

        protected void ddl_selection_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}