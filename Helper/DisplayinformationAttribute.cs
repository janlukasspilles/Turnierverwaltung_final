using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turnierverwaltung_final.Helper
{
    public class DisplayMetaInformation : Attribute
    {
        #region Attributes
        private int _order;
        private string _displayname;
        private bool _editable;
        private ControlType _controlType;
        #endregion
        #region Properties
        public int Order { get => _order; set => _order = value; }
        public string Displayname { get => _displayname; set => _displayname = value; }
        public bool Editable { get => _editable; set => _editable = value; }
        private ControlType ControlType { get => _controlType; set => _controlType = value; }
        #endregion
        #region Constructors
        public DisplayMetaInformation(string displayname, int order, bool editable, ControlType controlType)
        {
            ControlType = controlType;
            Editable = editable;
            Order = order;
            Displayname = displayname;
        }
        #endregion
        #region Methods
        #endregion
    }

    public enum ControlType
    {
        ctDomain,
        ctEdit,
        ctCheck
    }
}