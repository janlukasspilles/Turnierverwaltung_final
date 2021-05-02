//Autor:        Jan-Lukas Spilles
//Klasse:       IA119
//Datei:        Controller.cs
//Datum:        19.11.2020
//Beschreibung: Kümmert sich um den Programmablauf
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung.Model.TeilnehmerNS
{
    [Serializable]
    public abstract class Teilnehmer
    {
        #region Attributes
        private long _id;
        private string _name;
        #endregion
        #region Properties
        [DisplayMetaInformation("ID", 0, false, ControlType.ctEdit)]
        public long Id { get => _id; set => _id = value; }
        [DisplayMetaInformation("Name", 1, true, ControlType.ctEdit)]
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
