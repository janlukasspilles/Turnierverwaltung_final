using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using Turnierverwaltung.Model.TeilnehmerNS;
using Turnierverwaltung_final.Model;
using Turnierverwaltung_final.Model.TeilnehmerNS.Personen;

namespace Turnierverwaltung.ControllerNS
{
    public class Controller
    {
        #region Attributes
        private List<Teilnehmer> _teilnehmer;
        private Teilnehmer _neuerTeilnehmer;
        private List<Sportart> _sportarten;
        private List<Person> _moeglicheMitglieder;
        #endregion
        #region Properties
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        public Teilnehmer NeuerTeilnehmer { get => _neuerTeilnehmer; set => _neuerTeilnehmer = value; }
        public List<Sportart> Sportarten { get => _sportarten; set => _sportarten = value; }
        #endregion
        #region Constructors
        public Controller()
        {
            Teilnehmer = new List<Teilnehmer>();
            Sportarten = new List<Sportart>();
            GetAlleSportenarten();
        }
        #endregion
        #region Methods
        public List<Person> GetMoeglicheMitglieder(long mannschaftId)
        {
            List<Person> result = new List<Person>();
            Person p = null;
            string sql = "SELECT P.ID, " +
                "CASE " +
                "WHEN((SELECT 1 FROM TRAINER T WHERE T.PERSON_ID = P.iD) IS NOT NULL) THEN 'Trainer' " +
                "WHEN((SELECT 1 FROM PHYSIO PH WHERE PH.PERSON_ID = P.iD) IS NOT NULL) THEN 'Physio' " +
                "WHEN((SELECT 1 FROM FUSSBALLSPIELER FS WHERE FS.PERSON_ID = P.iD) IS NOT NULL) THEN 'Fussballspieler' " +
                "WHEN((SELECT 1 FROM HANDBALLSPIELER HS WHERE HS.PERSON_ID = P.iD) IS NOT NULL) THEN 'Handballspieler' " +
                "WHEN((SELECT 1 FROM TENNISSPIELER TS WHERE TS.PERSON_ID = P.iD) IS NOT NULL) THEN 'Tennisspieler' " +
                "ELSE 'Der Hund hat keine Detailtabelle' " +
                "END AS Profession " +
                "FROM PERSON P " +
                "LEFT OUTER JOIN personen_mannschaften pm " +
                "ON P.ID = pm.PERSON_ID " +
                $"WHERE pm.MANNSCHAFT_ID <> {mannschaftId} " +
                "OR pm.PERSON_ID IS NULL";

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
                        case "Trainer":
                            p = new Trainer();
                            break;
                        case "Physio":
                            p = new Physio();
                            break;
                        case "Fussballspieler":
                            p = new Fussballspieler();
                            break;
                        case "Handballspieler":
                            p = new Handballspieler();
                            break;
                        case "Tennisspieler":
                            p = new Tennisspieler();
                            break;
                    }
                    p.SelektionId(reader.GetInt64("ID"));
                    result.Add(p);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                con.Close();
            }

            return result;
        }
        public void GetAlleSportenarten()
        {
            Sportarten.Clear();
            string sql = "SELECT ID FROM SPORTART";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sportart s = new Sportart();
                    s.SelektionId(reader.GetInt64("ID"));
                    Sportarten.Add(s);
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
        public void GetAlleFussballspieler()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN FUSSBALLSPIELER FS ON P.ID = FS.PERSON_ID";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Fussballspieler t = new Fussballspieler();
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
        public void GetAlleHandballspieler()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN HANDBALLSPIELER HS ON P.ID = HS.PERSON_ID";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Handballspieler t = new Handballspieler();
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
        public void GetAlleTennisspieler()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN TENNISSPIELER TS ON P.ID = TS.PERSON_ID";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Tennisspieler t = new Tennisspieler();
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
        public void GetAllePersonen()
        {
            Teilnehmer.Clear();
            Person p = null;
            string sql = "SELECT P.ID, " +
                "case " +
                "when((SELECT 1 FROM TRAINER T WHERE T.PERSON_ID = P.ID) IS NOT NULL) THEN 'Trainer' " +
                "when((SELECT 1 FROM PHYSIO PH WHERE PH.PERSON_ID = P.ID) IS NOT NULL) THEN 'Physio' " +
                "when((SELECT 1 FROM FUSSBALLSPIELER FS WHERE FS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Fussballspieler' " +
                "when((SELECT 1 FROM HANDBALLSPIELER HS WHERE HS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Handballspieler' " +
                "when((SELECT 1 FROM TENNISSPIELER TS WHERE TS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Tennisspieler' " +
                "END AS Profession " +
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
                        case "Trainer":
                            p = new Trainer();
                            break;
                        case "Physio":
                            p = new Physio();
                            break;
                        case "Fussballspieler":
                            p = new Fussballspieler();
                            break;
                        case "Handballspieler":
                            p = new Handballspieler();
                            break;
                        case "Tennisspieler":
                            p = new Tennisspieler();
                            break;
                    }
                    p.SelektionId(reader.GetInt64("ID"));
                    Teilnehmer.Add(p);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                con.Close();
            }

        }
        public void GetAlleMannschaften()
        {
            Teilnehmer.Clear();
            string sql = "SELECT M.ID FROM MANNSCHAFT M";
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Mannschaft m = new Mannschaft();
                    m.SelektionId(reader.GetInt64("ID"));
                    Teilnehmer.Add(m);
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

        public IList GetDomainList(ddlList listname)
        {
            switch (listname)
            {
                case ddlList.dlSportarten:
                    return Sportarten;
                default: return null;
            }
        }
        #endregion
    }
    public enum ddlList
    {
        dlSportarten
    }
}
