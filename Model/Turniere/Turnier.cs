using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turnierverwaltung.Model.TeilnehmerNS;

namespace Turnierverwaltung_final.Model.Turniere
{
    public class Turnier
    {
        #region Attributes      
        private int _id;
        private List<Spiel> _spiele;
        private int _sportartId;
        private string _turnierName;
        private List<Teilnehmer> _teilnehmer;
        #endregion
        #region Properties
        public List<Spiel> Spiele { get => _spiele; set => _spiele = value; }
        public int SportartId { get => _sportartId; set => _sportartId = value; }
        public string Turniername { get => _turnierName; set => _turnierName = value; }
        public int Id { get => _id; set => _id = value; }
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        #endregion
        #region Constructors
        public Turnier()
        {

        }
        #endregion
        #region Methods
        #endregion
    }
}