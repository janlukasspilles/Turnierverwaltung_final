using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TVModelLib.Model;
using TVModelLib;
using TVModelLib.Model.TurniereNS;
using TVModelLib.Model.TeilnehmerNS;
using TVModelLib.Model.TeilnehmerNS.Personen;
using TVModelLib.Model.Benutzeranmeldung;
using System.Data;

namespace Turnierverwaltung.ControllerNS
{
    public class Controller
    {
        #region Attributes
        private List<Teilnehmer> _teilnehmer;
        private List<Turnier> _turniere;
        private List<Benutzer> _benutzer;
        #endregion
        #region Properties
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        public List<Turnier> Turniere { get => _turniere; set => _turniere = value; }
        public List<Benutzer> Benutzer { get => _benutzer; set => _benutzer = value; }
        #endregion
        #region Constructors
        public Controller()
        {
            Teilnehmer = new List<Teilnehmer>();
            Turniere = new List<Turnier>();
            Benutzer = new List<Benutzer>();
        }
        #endregion
        #region Methods   
        public bool CheckLogin(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(GlobalConstants.connectionString))
            {
                MySqlCommand command = new MySqlCommand("CheckPassword;", conn);
                command.CommandType = CommandType.StoredProcedure;

                // Add your parameters here if you need them
                command.Parameters.Add(new MySqlParameter("@BENUTZERNAME_b", username));
                command.Parameters.Add(new MySqlParameter("@PASSWORT_p", password));

                var returnParameter = command.Parameters.Add("@ReturnVal", MySqlDbType.Int32);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();

                _ = command.ExecuteNonQuery();
                return Convert.ToBoolean(returnParameter.Value);
            }
        }

        public List<Benutzerrolle> GetAlleBenutzerRollen()
        {
            string sql = "SELECT ID FROM BENUTZERROLLE";
            List<Benutzerrolle> result = new List<Benutzerrolle>();
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Benutzerrolle b = new Benutzerrolle();
                    b.SelektionId(reader.GetInt64("ID"));
                    result.Add(b);
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

        public void GetAlleBenutzer()
        {
            Benutzer.Clear();
            string sql = "SELECT B.ID FROM BENUTZER B";
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Benutzer b = new Benutzer();
                    b.SelektionId(reader.GetInt64("ID"));
                    Benutzer.Add(b);
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

        public DataTable GetRanking(int turnierId)
        {
            DataTable dt = new DataTable();
            var sql = "SELECT @`curRow` := @`curRow` + 1 AS \"Platzierung\""
                    + " ,FINAL.*"
                    + " FROM"
                    + " ("
                        + " SELECT  SUBVALUES.NAME"
                        + " ,SUM(SUBVALUES.Gewonnen * 3) + SUM(SUBVALUES.Unentschieden * 1) / 2 AS \"Punkte\""
                        + " ,SUBVALUES.Gewonnen"
                        + " ,SUBVALUES.Verloren"
                        + " ,SUBVALUES.Unentschieden"
                        + " ,SUBVALUES.Differenz AS \"Tordifferenz\""
                        + " FROM"
                        + " ("
                            + " SELECT  SUBSTATUS.NAME"
                            + " ,SUM(if (SUBSTATUS.STATUS = 'Gewonnen',1,0))      AS 'Gewonnen'"
                            + " ,SUM(if (SUBSTATUS.STATUS = 'Verloren',1,0))      AS 'Verloren'"
                            + " ,SUM(if (SUBSTATUS.STATUS = 'Unentschieden',1,0)) AS 'Unentschieden'"
                            + " ,SUM(SUBSTATUS.Differenz)                        AS 'Differenz'"

                            + " FROM"
                            + " ("
                                + " SELECT  m.NAME"
                                + " , ms.PUNKTE_MANNSCHAFT1 - ms.PUNKTE_MANNSCHAFT2 AS \"Differenz\""
                                + " , CASE WHEN ms.PUNKTE_MANNSCHAFT1 > ms.PUNKTE_MANNSCHAFT2 THEN 'Gewonnen'"
                                    + " WHEN ms.PUNKTE_MANNSCHAFT1 < ms.PUNKTE_MANNSCHAFT2 THEN 'Verloren'  ELSE 'Unentschieden' END AS \"Status\""
                                + " FROM MANNSCHAFT M"
                                + " JOIN mannschaftsspiel ms"
                                + " ON ms.MANNSCHAFT1_ID = m.ID"
                                + $" WHERE ms.TURNIER_ID = '{turnierId}'"
                                + " UNION ALL"
                                + " SELECT m.NAME"
                                + " , ms.PUNKTE_MANNSCHAFT2 - ms.PUNKTE_MANNSCHAFT1 AS \"Differenz\""
                                + " , CASE WHEN ms.PUNKTE_MANNSCHAFT1 < ms.PUNKTE_MANNSCHAFT2 THEN 'Gewonnen'"
                                    + " WHEN ms.PUNKTE_MANNSCHAFT1 > ms.PUNKTE_MANNSCHAFT2 THEN 'Verloren'  ELSE 'Unentschieden' END AS \"Status\""
                                + " FROM MANNSCHAFT M"
                                + " JOIN mannschaftsspiel ms"
                                + " ON ms.MANNSCHAFT2_ID = m.ID"
                                + $" WHERE ms.TURNIER_ID = '{turnierId}'"
                            + " ) AS SUBSTATUS"
                            + " GROUP BY  SUBSTATUS.NAME"
                    + " ) AS SUBVALUES"
                    + " GROUP BY  SUBVALUES.NAME"
                    + " ORDER BY Punkte desc"
                            + " ,Differenz desc"
                + " ) AS FINAL, ("
                + " SELECT  @`curRow`:= 0) r; ";

            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        //cmd.Parameters.AddWithValue("@curRow", 0);
                        dt.Load(cmd.ExecuteReader());
                        return dt;
                    }
                }
                catch (MySqlException e)
                {
#if DEBUG
                    Debug.WriteLine(e.Message);
#endif
                    throw e;
                }
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
                        when((SELECT 1 FROM MATERIALWART MW WHERE MW.PERSON_ID = P.ID) IS NOT NULL) THEN 'Materialwart'
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
                        case "Materialwart":
                            p = new Materialwart();
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
                "when((SELECT 1 FROM MATERIALWART MW WHERE MW.PERSON_ID = P.ID) IS NOT NULL) THEN 'Materialwart' " +
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
                        case "Materialwart":
                            p = new Materialwart();
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

        public void GetAlleMaterialwarte()
        {
            Teilnehmer.Clear();
            string sql = "SELECT P.ID FROM PERSON P JOIN MATERIALWART M ON P.ID = M.PERSON_ID";
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Materialwart t = new Materialwart();
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
