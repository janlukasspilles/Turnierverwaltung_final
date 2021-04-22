using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Turnierverwaltung_final.Helper;
using Turnierverwaltung_final.Model.Spieler;

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
        }
        #endregion
        #region Methods
        public void GetAllePhysios()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN PHYSIO PH ON P.ID = PH.PERSON_ID";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Physio p = new Physio();
                    p.SelektionId(reader.GetInt64("ID"));
                    Teilnehmer.Add(p);
                }
                reader.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                con.Close();
            }
        }
        public void GetAlleTrainer()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN TRAINER T ON P.ID = T.PERSON_ID";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Trainer t = new Trainer();
                    t.SelektionId(reader.GetInt64("ID"));
                    Teilnehmer.Add(t);
                }
                reader.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                con.Close();
            }
        }
        public void GetAlleSpieler()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN SPIELER SP ON P.ID = SP.PERSON_ID";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Spieler s = new Spieler();
                    s.SelektionId(reader.GetInt64("ID"));
                    Teilnehmer.Add(s);
                }
                reader.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                con.Close();
            }
        }

        public void GetAllePersonen()
        {
            Teilnehmer.Clear();
            Person p = null;
            string sql = "SELECT P.ID," +
                " case " +
                "when((SELECT 1 from TRAINER T where T.PERSON_ID = P.id) is not null) then 'Trainer' " +
                "when((SELECT 1 from SPIELER SP where SP.PERSON_ID = P.id) is not null) then 'Spieler' " +
                "when((SELECT 1 from PHYSIO PH where PH.PERSON_ID = P.id) is not null) then 'Physio' " +
                "end as Profession " +
                "FROM PERSON P";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    switch (reader.GetString("Profession"))
                    {
                        case "Spieler":
                            p = new Spieler();
                            break;
                        case "Trainer":
                            p = new Trainer();
                            break;
                        case "Physio":
                            p = new Physio();
                            break;
                    }
                    p.SelektionId(reader.GetInt64("ID"));
                    Teilnehmer.Add(p);
                }
                reader.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                con.Close();
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
            foreach (Teilnehmer t in Teilnehmer)
            {
                if (t is Mannschaft)
                    res.Add(t);
            }
            return res;
        }
        #endregion
    }
}
