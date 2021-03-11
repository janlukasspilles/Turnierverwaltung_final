namespace Turnierverwaltung
{
    public abstract class Spieler : Teilnehmer
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
        #region Constructors
        public Spieler() : base()
        {

        }
        #endregion
        #region Methods       

        public override string GetInformation()
        {
            return base.GetInformation() + $"{Geburtstag};{Mannschaft};";
        }

        public abstract override bool Speichern();
        public abstract override void SelektionId(long id);
        public abstract override bool Neuanlage();
        public abstract override bool Loeschen();
        #endregion
    }
}
