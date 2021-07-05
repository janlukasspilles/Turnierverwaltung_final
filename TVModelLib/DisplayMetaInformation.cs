using System;

namespace TVModelLib
{
    public class DisplayMetaInformation : Attribute 
    {
        #region Attributes
        private int _order;
        private string _displayname;
        private bool _editable;
        private ControlType _controlType;
        private DdlList _domainList;
        private string _domainName;
        #endregion
        #region Properties
        public int Order { get => _order; set => _order = value; }
        public string Displayname { get => _displayname; set => _displayname = value; }
        public bool Editable { get => _editable; set => _editable = value; }
        public ControlType ControlType { get => _controlType; set => _controlType = value; }
        public DdlList DomainList { get => _domainList; set => _domainList = value; }
        public string DomainName { get => _domainName; set => _domainName = value; }
        #endregion
        #region Constructors
        public DisplayMetaInformation(string displayname, int order, bool editable, ControlType controlType)
        {
            ControlType = controlType;
            Editable = editable;
            Order = order;
            Displayname = displayname;
        }
        public DisplayMetaInformation(string displayname, int order, bool editable, ControlType controlType, DdlList domainList)
        {
            ControlType = controlType;
            Editable = editable;
            Order = order;
            Displayname = displayname;
            DomainList = domainList;
        }
        #endregion
        #region Methods
        #endregion
    }
}