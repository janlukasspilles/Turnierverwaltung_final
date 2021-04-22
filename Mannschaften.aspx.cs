using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Turnierverwaltung_final
{
    public partial class Mannschaften : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        // Es gibt eine Tabelle mit allen Mannschaften analog zur Personenansicht
        // Es gibt einen neuen Button "Spieler hinzufügen" in der Tabelle für jede Mannschaft
        // Wird dieser Button gedrückt öffnen sich zwei Felder
        // Feld1 sind die Mitglieder der entsprechenden Mannschaft
        // Feld2 zeigt alle Personen, die Mitglieder dieser Mannschaft werden können
        // Spieler in den jeweiligen Feldern lassen sich auswählen
        // Zwei Buttons über die Spieler hinzugefügt oder entfernt werden können
    }
}