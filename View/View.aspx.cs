using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung;

namespace Turnierverwaltung_final
{
    public partial class View : Page
    {
        private List<Teilnehmer> _teilnehmer;
        private List<string> _properties;
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        public List<string> Properties { get => _properties; set => _properties = value; }

        public View() : base()
        {
            Teilnehmer = Global.Controller.GetAlleTeilnehmer();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Refresh_Click(object sender, EventArgs e)
        {
            Teilnehmer = Global.Controller.GetAlleTeilnehmer();
        }

        protected void btn_Insert_Click(object sender, EventArgs e)
        {
            Fussballspieler fs = new Fussballspieler() { Vorname = txt_Vorname.Text, Nachname = txt_Nachname.Text, Geburtstag = txt_Geburtstag.Text, Mannschaft = txt_Mannschaft.Text, AnzahlTore = Convert.ToInt32(txt_AnzahlTore.Text) };
            Global.Controller.TeilnehmerHinzufuegen(fs);
        }
    }
}