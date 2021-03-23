//Autor:        Jan-Lukas Spilles
//Klasse:       IA119
//Datei:        Controller.cs
//Datum:        19.11.2020
//Beschreibung: Kümmert sich um den Programmablauf
using System.ComponentModel;

namespace Turnierverwaltung
{
    public abstract class Teilnehmer
    {
        #region Attributes
        private long _id;
        private string _name;
        #endregion
        #region Properties
        public long Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        #endregion
        #region Constructors
        public Teilnehmer()
        {
            
        }
        #endregion
        #region Methods
        public abstract bool Speichern();
        public abstract void SelektionId(long id);
        public abstract bool Neuanlage();
        public abstract bool Loeschen();        
        #endregion
    }
}
