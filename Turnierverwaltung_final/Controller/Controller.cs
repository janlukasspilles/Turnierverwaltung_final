using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TVModeLib.Model.TeilnehmerNS;
using TVModeLib.Model;
using TVModeLib.Model.TeilnehmerNS.Personen;
using TVModeLib.Model.TurniereNS;
using TVModelLib;

namespace Turnierverwaltung.ControllerNS
{
    public class Controller
    {
        #region Attributes
        private List<Teilnehmer> _teilnehmer;
        private Teilnehmer _neuerTeilnehmer;
        private List<Sportart> _sportarten;
        private List<Person> _moeglicheMitglieder;
        private List<Turnierart> _turnierarten;
        private List<Turnier> _turniere;
        private Turnier _neuesTurnier;
        #endregion
        #region Properties
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        public Teilnehmer NeuerTeilnehmer { get => _neuerTeilnehmer; set => _neuerTeilnehmer = value; }
        public List<Sportart> Sportarten { get => _sportarten; set => _sportarten = value; }
        public List<Turnierart> Turnierarten { get => _turnierarten; set => _turnierarten = value; }
        public List<Turnier> Turniere { get => _turniere; set => _turniere = value; }
        public Turnier NeuesTurnier { get => _neuesTurnier; set => _neuesTurnier = value; }
        public List<Person> MoeglicheMitglieder { get => _moeglicheMitglieder; set => _moeglicheMitglieder = value; }
        #endregion
        #region Constructors
        public Controller()
        {
            Teilnehmer = new List<Teilnehmer>();
            Sportarten = new List<Sportart>();
            Turnierarten = new List<Turnierart>();
            Turniere = new List<Turnier>();
            GetAlleSportenarten();
            GetAlleTurnierArten();
        }
        #endregion
        #region Methods     
        public void GetAlleTurniere()
        {
            Turniere.Clear();
            string sql = "SELECT T.ID FROM TURNIER T";
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Turnier t = new Turnier();
                    t.SelektionId(reader.GetInt64("ID"));
                    Turniere.Add(t);
                }
                reader.Close();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
        public List<Teilnehmer> GetMoeglicheMitglieder(long mannschaftId)
        {
            List<Teilnehmer> result = new List<Teilnehmer>();
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

            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
        public void GetAlleTurnierArten()
        {
            Turnierarten.Clear();
            string sql = "SELECT ID FROM TURNIERART";
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Turnierart t = new Turnierart();
                    t.SelektionId(reader.GetInt64("ID"));
                    Turnierarten.Add(t);
                }
                reader.Close();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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

            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
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
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
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
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
        public IList GetDomainList(DdlList listname)
        {
            switch (listname)
            {
                case DdlList.dlSportarten:
                    return Sportarten;
                case DdlList.dlTurnierarten:
                    return Turnierarten;
                default: return null;
            }
        }
        #endregion
    }
}
