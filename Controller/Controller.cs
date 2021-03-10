using System.Collections.Generic;

namespace Turnierverwaltung.ControllerNS
{
    public class Controller
    {
        #region Attributes
        private List<Teilnehmer> _teilnehmer;
        #endregion
        #region Properties
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        #endregion
        #region Constructors
        public Controller()
        {
            Teilnehmer = new List<Teilnehmer>();
            Fussballspieler f1 = new Fussballspieler
            {
                Vorname = "Peter",
                Nachname = "Zwegat",
                AnzahlTore = 10,
                Geburtstag = "1996-11-19",
                Mannschaft = "1. Fc Köln"
            };
            TeilnehmerHinzufuegen(f1);
            f1.AnzahlTore = 11;
            TeilnehmerAendern(f1.Id);
            TeilnehmerLoeschen(f1.Id);
        }
        #endregion
        #region Methods
        public bool TeilnehmerHinzufuegen(Teilnehmer t)
        {
            if (t.Neuanlage())
            {
                Teilnehmer.Add(t);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TeilnehmerAendern(long id)
        {
            foreach (Teilnehmer t in Teilnehmer)
            {
                if (t.Id == id)
                {
                    return t.Speichern();
                }
                else
                {
                    //Nichts
                }
            }
            return false;
        }
        public bool TeilnehmerLoeschen(long id)
        {
            foreach (Teilnehmer t in Teilnehmer)
            {
                if (t.Id == id)
                {
                    if (t.Loeschen())
                    {
                        Teilnehmer.Remove(t);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //Nichts
                }
            }
            return false;
        }
        public List<Teilnehmer> GetAlleTeilnehmer()
        {
            return _teilnehmer;
        }

        public bool MannschaftHinzufuegen(Mannschaft m)
        {
            if (m.Neuanlage())
            {
                Teilnehmer.Add(m);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MannschaftAendern(long id)
        {
            foreach (Mannschaft m in Teilnehmer)
            {
                if (m.Id == id)
                {
                    return m.Speichern();
                }
                else
                {
                    //Nichts
                }
            }
            return false;
        }

        public bool MannschaftLoeschen(long id)
        {
            foreach (Mannschaft m in Teilnehmer)
            {
                if (m.Id == id)
                {
                    if (m.Loeschen())
                    {
                        Teilnehmer.Remove(m);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //Nichts
                }
            }
            return false;
        }

        public List<Teilnehmer> GetAlleMannschaften()
        {
            List<Teilnehmer> res = new List<Teilnehmer>();
            foreach(Teilnehmer t in Teilnehmer)
            {
                if (t is Mannschaft)
                    res.Add(t);
            }
            return res;
        }
        #endregion
    }
}
