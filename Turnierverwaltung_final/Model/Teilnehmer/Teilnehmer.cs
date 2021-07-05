//Autor:        Jan-Lukas Spilles
//Klasse:       IA119
//Datei:        Controller.cs
//Datum:        19.11.2020
//Beschreibung: Kümmert sich um den Programmablauf
using System;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung.Model.TeilnehmerNS
{
    [Serializable]
    public abstract class Teilnehmer : IEquatable<Teilnehmer>
    {
        #region Attributes
        private long _id;
        private string _name;
        #endregion
        #region Properties
        [DisplayMetaInformation("ID", 0, false, ControlType.ctEditText)]
        public long Id { get => _id; set => _id = value; }
        [DisplayMetaInformation("Name", 1, true, ControlType.ctEditText)]
        public string Name { get => _name; set => _name = value; }
        #endregion
        #region Constructors
        public Teilnehmer()
        {
            Name = "";
        }
        #endregion
        #region Methods
        public abstract bool Speichern();
        public abstract void SelektionId(long id);
        public abstract bool Neuanlage();
        public abstract bool Loeschen();
        public override bool Equals(object obj)
            => Equals(obj as Teilnehmer);
        public override string ToString() 
            => Name;
        public override int GetHashCode() 
            => Id.GetHashCode();

        public bool Equals(Teilnehmer other)
        {
            return other != null && GetType().Equals(other.GetType()) && ((Teilnehmer)other).Id == Id;
        }
        #endregion
    }
}
