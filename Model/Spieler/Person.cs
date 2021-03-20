namespace Turnierverwaltung
{
    public abstract class Person : Teilnehmer
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
        public Person() : base()
        {

        }
        #endregion
        #region Methods       
        public abstract override bool Speichern();
        public abstract override void SelektionId(long id);
        public abstract override bool Neuanlage();
        public abstract override bool Loeschen();
        #endregion
    }
}
