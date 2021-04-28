//Autor:        Jan-Lukas Spilles
//Klasse:       IA119
//Datei:        Controller.cs
//Datum:        19.11.2020
//Beschreibung: Kümmert sich um den Programmablauf
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Turnierverwaltung.Model
{
    public abstract class Teilnehmer
    {
        #region Attributes
        private long _id;
        private string _name;
        #endregion
        #region Properties
        [Display(Name="ID", Order = 0)]
        public long Id { get => _id; set => _id = value; }
        [Display(Name = "Name", Order = 1)]
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
