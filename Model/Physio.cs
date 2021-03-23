using System;

namespace Turnierverwaltung
{
    public class Physio : Teilnehmer
    {
        #region Attributes 
        private string _vorname;
        private string _nachname;
        private string _geburtstag;
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
        #endregion
        #region Constructors
        public Physio() : base()
        {

        }
        #endregion
        #region Methods

        public override bool Speichern()
        {
            throw new NotImplementedException();
        }

        public override void SelektionId(long id)
        {
            throw new NotImplementedException();
        }

        public override bool Neuanlage()
        {
            throw new NotImplementedException();
        }

        public override bool Loeschen()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
