using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.Model;
using Turnierverwaltung_final.Helper;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;
using Turnierverwaltung_final.Model.Personen;

namespace Turnierverwaltung_final.View
{
    public partial class Mannschaften : Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            Controller.GetAlleMannschaften();
            LoadTable();
            if (IsPostBack)
            {
                if ((pnl_tbl.FindControl("tbl_mannschaften") as CustomTable).HasNewEntry)
                {
                    (pnl_tbl.FindControl("tbl_mannschaften") as CustomTable).GenerateNewEntryRow();
                }
                foreach (TableRow r in (pnl_tbl.FindControl("tbl_mannschaften") as CustomTable).Rows)
                {
                    if (r is CustomRow)
                    {
                        if ((r as CustomRow).RowState == RowState.rsEdit)
                        {
                            (r as CustomRow).RefreshRow();
                        }
                        (r as CustomRow).EditButton.Click += EditMannschaften;
                    }
                }
            }
        }

        //private delegate void Action<string> Beispiel
        //{
        //    Console.WriteLine("Hier!" + halo);  
        //}

        private void LoadTable()
        {
            pnl_tbl.Controls.Add(new CustomTable(Controller.Teilnehmer, "tbl_mannschaften"));
        }
        // Es gibt eine Tabelle mit allen Mannschaften analog zur Personenansicht
        // Es gibt einen neuen Button "Spieler hinzufügen" in der Tabelle für jede Mannschaft
        // Wird dieser Button gedrückt öffnen sich zwei Felder
        // Feld1 sind die Mitglieder der entsprechenden Mannschaft
        // Feld2 zeigt alle Personen, die Mitglieder dieser Mannschaft werden können
        // Spieler in den jeweiligen Feldern lassen sich auswählen
        // Zwei Buttons über die Spieler hinzugefügt oder entfernt werden können
        private void EditMannschaften(object sender, EventArgs e)
        {
            ListBox membersIn = new ListBox() { CssClass ="table table-bordered"};
            Mannschaft currentMannschaft = (pnl_tbl.FindControl("tbl_mannschaften") as CustomTable).Content[Convert.ToInt32((sender as Button).CommandArgument) - 1] as Mannschaft;
            currentMannschaft.SelektiereMitgliederInListe();
            foreach (Person p in currentMannschaft.Mitglieder)
            {
                membersIn.Items.Add(p.ToString());
            }
            pnl_mitgliederswitch.Controls.Add(membersIn);
        }
    }
}