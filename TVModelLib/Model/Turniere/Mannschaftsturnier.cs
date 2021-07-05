using System.Collections.Generic;
using TVModeLib.Model.TeilnehmerNS;

namespace TVModeLib.Model.TurniereNS
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