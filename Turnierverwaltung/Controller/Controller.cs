using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TVModelLib.Model;
using TVModelLib;
using TVModelLib.Model.TurniereNS;
using TVModelLib.Model.TeilnehmerNS;
using TVModelLib.Model.TeilnehmerNS.Personen;

namespace Turnierverwaltung.ControllerNS
{
    public class Controller
    {
        #region Attributes
        private List<Teilnehmer> _teilnehmer;
        private List<Turnier> _turniere;
        #endregion
        #region Properties
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        public List<Turnier> Turniere { get => _turniere; set => _turniere = value; }
        #endregion
        #region Constructors
        public Controller()
        {
            Teilnehmer = new List<Teilnehmer>();
            Turniere = new List<Turnier>();
        }
        #endregion
        #region Methods     

        public void GetRanking(int turnierId)
        {

        }

        public List<Turnierart> GetAlleTurnierarten()
        {
            string sql = "SELECT ID FROM TURNIERART";
            List<Turnierart> result = new List<Turnierart>();
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
                    result.Add(t);
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

        public List<Sportart> GetAlleSportarten()
        {
            string sql = "SELECT ID FROM SPORTART";
            List<Sportart> result = new List<Sportart>();
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
                    result.Add(s);
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
        public List<Teilnehmer> GetMoeglicheTurnierTeilnehmerEinzel(long turnierId)
        {
            List<Teilnehmer> result = new List<Teilnehmer>();
            Person p = null;
            string sql = $@"SELECT P.ID,
                        CASE 
                        WHEN((SELECT 1 FROM TRAINER T WHERE T.PERSON_ID = P.iD) IS NOT NULL) THEN 'Trainer'
                        WHEN((SELECT 1 FROM PHYSIO PH WHERE PH.PERSON_ID = P.iD) IS NOT NULL) THEN 'Physio'
                        WHEN((SELECT 1 FROM FUSSBALLSPIELER FS WHERE FS.PERSON_ID = P.iD) IS NOT NULL) THEN 'Fussballspieler' 
                        WHEN((SELECT 1 FROM HANDBALLSPIELER HS WHERE HS.PERSON_ID = P.iD) IS NOT NULL) THEN 'Handballspieler' 
                        WHEN((SELECT 1 FROM TENNISSPIELER TS WHERE TS.PERSON_ID = P.iD) IS NOT NULL) THEN 'Tennisspieler' 
                        ELSE 'Der Hund hat keine Detailtabelle' 
                        END AS Profession 
                        FROM PERSON P
                        LEFT OUTER JOIN TURNIER_PERSON TP 
                        ON P.ID = TP.PERSON_ID
                        WHERE TP.TURNIER_ID IS NULL
                        OR TP.TURNIER_ID <> {turnierId}";

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
        public List<Teilnehmer> GetMoeglicheTurnierteilnehmer(long turnierId, int turnierart)
        {
            switch (turnierart)
            {
                case 1:
                    return GetMoeglicheTurnierTeilnehmermannschaft(turnierId);
                case 2:
                    return GetMoeglicheTurnierTeilnehmerEinzel(turnierId);
                default: throw new Exception($"Nicht gemappte oder ungültige Turnierart: {turnierart}");
            }
        }

        public List<Teilnehmer> GetMoeglicheTurnierTeilnehmermannschaft(long turnierId)
        {
            List<Teilnehmer> result = new List<Teilnehmer>();
            string sql = $@"SELECT * 
                            FROM MANNSCHAFT M
                            WHERE NOT EXISTS 
                                (SELECT * 
                                FROM TURNIER_MANNSCHAFT TM 
                                WHERE TM.MANNSCHAFT_ID = M.ID 
                                AND TM.TURNIER_ID = {turnierId})";

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
                    result.Add(m);
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
        #endregion
    }
}
