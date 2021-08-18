using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using TVModelLib.Model.TurniereNS;
using TVModelLib;
// Es gibt eine Tabelle mit allen Mannschaften analog zur Personenansicht
// Es gibt einen neuen Button "Spieler hinzufügen" in der Tabelle für jede Mannschaft
// Wird dieser Button gedrückt öffnen sich zwei Felder
// Feld1 sind die Mitglieder der entsprechenden Mannschaft
// Feld2 zeigt alle Personen, die Mitglieder dieser Mannschaft werden können
// Spieler in den jeweiligen Feldern lassen sich auswählen
// Zwei Buttons über die Spieler hinzugefügt oder entfernt werden können
namespace Turnierverwaltung.View.Pages
{
    public partial class Ranking : Page
    {
        #region Attributes
        private Controller _controller;
        #endregion
        #region Properties
        public Controller Controller { get => _controller; set => _controller = value; }
        #endregion
        #region Constructors
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            Controller.GetAlleTurniere();
            ddlTurnier.DataSource = Controller.Turniere;
            ddlTurnier.DataBind();
            gvRanking.DataSource = Controller.GetRanking(1);
            gvRanking.DataBind();
        }
        #endregion
    }
}