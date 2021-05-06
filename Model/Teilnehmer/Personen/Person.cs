using System;
using System.ComponentModel.DataAnnotations;
using Turnierverwaltung.Model.TeilnehmerNS;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung_final.Model.TeilnehmerNS.Personen
{
    [Serializable]
    public abstract class Person : Teilnehmer
    {
        #region Attributes      
        private string _vorname;
        private string _nachname;
        private string _geburtstag;
        #endregion
        #region Properties
        [DisplayMetaInformation("Vorname", 2, true, ControlType.ctEditText)]
        public string Vorname
        {
            get => _vorname;
            set
            {
                _vorname = value;
                Name = Vorname + " " + Nachname;
            }
        }
        [DisplayMetaInformation("Nachname", 3, true, ControlType.ctEditText)]
        public string Nachname
        {
            get => _nachname;
            set
            {
                _nachname = value;
                Name = Vorname + " " + Nachname;
            }
        }
        [DisplayMetaInformation("Geburtstag", 4, true, ControlType.ctEditText)]
        public string Geburtstag { get => _geburtstag; set => _geburtstag = value; }
        #endregion
        #region Constructs
        #endregion
        #region Methods
        public abstract override bool Loeschen();
        public abstract override bool Neuanlage();
        public abstract override void SelektionId(long id);
        public abstract override bool Speichern();
        public override string ToString()
        {
            return base.ToString();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        #endregion
    }
}