using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turnierverwaltung.Model.TeilnehmerNS;

namespace Turnierverwaltung_final.Model.Turniere
{
    public class Spiel
    {
        #region Attributes
        private int _id;
        private int _turnierId;
        private int _punkteTeilnehmer1;
        private int _punkteTeilnehmer2;
        private Teilnehmer _teilnehmer1;
        private Teilnehmer _teilnehmer2;
        #endregion
        #region Properties
        public int PunkteTeilnehmer1 { get => _punkteTeilnehmer1; set => _punkteTeilnehmer1 = value; }
        public int PunkteTeilnehmer2 { get => _punkteTeilnehmer2; set => _punkteTeilnehmer2 = value; }
        public int Id { get => _id; set => _id = value; }
        public int TurnierId { get => _turnierId; set => _turnierId = value; }
        public Teilnehmer Teilnehmer1 { get => _teilnehmer1; set => _teilnehmer1 = value; }
        public Teilnehmer Teilnehmer2 { get => _teilnehmer2; set => _teilnehmer2 = value; }
        #endregion
        #region Constructors

        #endregion
        #region Methods
        #endregion
    }
}