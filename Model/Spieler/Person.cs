using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turnierverwaltung;

namespace Turnierverwaltung_final.Model.Spieler
{
    public abstract class Person : Teilnehmer
    {
        #region Attributes      
        private string _vorname;
        private string _nachname;
        private string _geburtstag;
        private string _mannschaft;
        #endregion
        #region Properties
        public string Vorname
        {
            get => _vorname;
            set
            {
                _vorname = value;
                Name = Vorname + " " + Nachname;
            }
        }
        public string Nachname
        {
            get => _nachname;
            set
            {
                _nachname = value;
                Name = Vorname + " " + Nachname;
            }
        }
        public string Geburtstag { get => _geburtstag; set => _geburtstag = value; }
        public string Mannschaft { get => _mannschaft; set => _mannschaft = value; }
        #endregion
        #region Constructs
        #endregion
        #region Methods
        public abstract override bool Loeschen();
        public abstract override bool Neuanlage();
        public abstract override void SelektionId(long id);
        public abstract override bool Speichern();
        #endregion
    }
}