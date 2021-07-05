using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turnierverwaltung.Model.TeilnehmerNS;

namespace Turnierverwaltung.Model.TurniereNS
{
    public class Mannschaftsturnier : Turnier
    {
        #region Attributes
        private List<Mannschaft> _mannschaften;
        #endregion
        #region Properties
        public List<Mannschaft> Mannschaften { get => _mannschaften; set => _mannschaften = value; }
        #endregion
        #region Constructors
        #endregion
        #region Methods
        #endregion
    }
}