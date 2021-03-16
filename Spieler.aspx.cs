using System;
using Turnierverwaltung;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung_final.View
{
    public partial class Spieler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Viewhelpers.CreateGenericTable(Global.Controller.Teilnehmer).ForEach(x => tblFussballer.Rows.Add(x));            
            Panel1.Controls.Add(new CustomTable<Teilnehmer>(Global.Controller.Teilnehmer));
        }
    }
}