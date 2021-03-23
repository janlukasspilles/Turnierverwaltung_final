using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turnierverwaltung_final.Model
{
    public class Sportart
    {
        #region Attributes
        private long _id;
        private string _bezeichnung;
        private int _anzahlPunkteSieg;
        private int _anzahlPunktePatt;
        private int _anzahlPunkteVerlust;
        private int _anzahlSpielerProTeam;
        #endregion
        #region Properties
        public long Id { get => _id; set => _id = value; }
        public string Bezeichnung { get => _bezeichnung; set => _bezeichnung = value; }
        public int AnzahlPunkteVerlust { get => _anzahlPunkteVerlust; set => _anzahlPunkteVerlust = value; }
        public int AnzahlPunktePatt { get => _anzahlPunktePatt; set => _anzahlPunktePatt = value; }
        public int AnzahlPunkteSieg { get => _anzahlPunkteSieg; set => _anzahlPunkteSieg = value; }
        public int AnzahlSpielerProTeam { get => _anzahlSpielerProTeam; set => _anzahlSpielerProTeam = value; }
        #endregion
        #region Constructors
        public Sportart()
        {

        }
        #endregion
        #region Methods
        public void Neuanlage()
        {
            string insertNewSportart = $"INSERT INTO SPORTART(BEZEICHNUNG, ANZAHL_PUNKTE_SIEG, ANZAHL_PUNKTE_VERLUST, ANZAHL_PUNKTE_PATT, ANZAHL_SPIELER_PRO_TEAM) VALUES ('{Bezeichnung}', {AnzahlPunkteSieg}, {AnzahlPunkteVerlust}, {AnzahlPunktePatt}, {AnzahlSpielerProTeam})";
            


        }
        #endregion
    }
}