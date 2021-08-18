using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TVModelLib.Model.TeilnehmerNS;
using TVModelLib.Database;

namespace TVModelLib.Model.TurniereNS
{
    [Serializable]
    public class Turnier
    {
        #region Attributes      
        private long _id;
        private List<Spiel> _spiele;
        private int _sportartId;
        private string _turnierName;
        private List<Teilnehmer> _turnierTeilnehmer;
        private int _turnierart;
        private bool _abgeschlossen;
        #endregion
        #region Properties
        public List<Spiel> Spiele { get => _spiele; set => _spiele = value; }
        [DisplayMetaInformation("Sportart", 17, true, ControlType.ctDomain, DdlList.dlSportarten, DomainName = "SPORTART")]
        public int SportartId { get => _sportartId; set => _sportartId = value; }
        [DisplayMetaInformation("Bezeichnung", 16, true, ControlType.ctEditText)]
        public string Turniername { get => _turnierName; set => _turnierName = value; }
        public long Id { get => _id; set => _id = value; }
        public List<Teilnehmer> TurnierTeilnehmer { get => _turnierTeilnehmer; set => _turnierTeilnehmer = value; }
        [DisplayMetaInformation("Turnierart", 18, true, ControlType.ctDomain, DdlList.dlTurnierarten, DomainName = "TURNIERART")]
        public int Turnierart { get => _turnierart; set => _turnierart = value; }
        [DisplayMetaInformation("Abgeschlossen", 19, false, ControlType.ctCheck)]
        public bool Abgeschlossen { get => _abgeschlossen; set => _abgeschlossen = value; }
        #endregion
        #region Constructors
        public Turnier()
        {
            Spiele = new List<Spiel>();
            TurnierTeilnehmer = new List<Teilnehmer>();
            SportartId = 1;
            Turniername = "";
            Id = 0;
            Turnierart = 1;
            Abgeschlossen = false;
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return Turniername;
        }
        public void Neuanlage()
        {
            bool res = true;
            string insertMannschaft = $"INSERT INTO TURNIER (BEZEICHNUNG, SPORTART_ID, TURNIERART_ID, ABGESCHLOSSEN) VALUES ('{Turniername}', '{SportartId}', '{Turnierart}', '0')";

            MySqlConnection Connection = new MySqlConnection(GlobalConstants.connectionString);
            Connection.Open();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection
            };

            try
            {
                command.CommandText = insertMannschaft;
                res = command.ExecuteNonQuery() == 1;
                Id = command.LastInsertedId;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                res = false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public void Speichern()
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string update = $"UPDATE TURNIER SET BEZEICHNUNG = '{Turniername}', SPORTART_ID = '{SportartId}', TURNIERART_ID = '{Turnierart}', ABGESCHLOSSEN = '{Abgeschlossen}' WHERE ID = '{Id}'";
                    using (MySqlCommand cmd = new MySqlCommand(update, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    if (TurnierTeilnehmer != null)
                        SpeichereTeilnehmer();
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
        public void Loeschen()
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string delete = $"DELETE FROM TURNIER WHERE ID = '{Id}'";
                    using (MySqlCommand cmd = new MySqlCommand(delete, con))
                    {
                        cmd.ExecuteNonQuery();
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
        public void SelektionId(long id)
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string selectionString = $"SELECT T.* " +
                        $"FROM TURNIER T " +
                        $"WHERE T.ID = '{id}'";

                    using (MySqlCommand cmd = new MySqlCommand(selectionString, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = reader.GetInt64("ID");
                                SportartId = reader.GetInt32("SPORTART_ID");
                                Turniername = reader.GetString("BEZEICHNUNG");
                                Turnierart = reader.GetInt32("TURNIERART_ID");
                                Abgeschlossen = reader.GetBoolean("ABGESCHLOSSEN");
                            }
                        }
                    }
                    Spiele = GetSpiele();
                    TurnierTeilnehmer = GetTeilnehmer();
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
        private List<Spiel> GetSpiele()
        {
            Spiele.Clear();
            List<Spiel> result = new List<Spiel>();
            string turnierart = DatabaseHelper.ReturnSingleValue("Bezeichnung", "turnierart", Turnierart).ToString();
            string sql = "";
            switch (turnierart)
            {
                case "Mannschaftsturnier":
                    sql = $"SELECT * FROM MANNSCHAFTSSPIEL MS JOIN TURNIER T ON T.ID = MS.TURNIER_ID WHERE T.ID = '{Id}'";
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
                            {
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Spiel s = new Spiel();
                                        s.SelektionId(reader.GetInt64("ID"), "Mannschaftsspiel");
                                        result.Add(s);
                                    }
                                }
                            }
                        }
                        catch (MySqlException e)
                        {
#if DEBUG
                            Debug.WriteLine(e.Message);
#endif
                        }
                    }
                    break;
                case "Einzelturnier":
                    sql = $"SELECT * FROM EINZELSPIEL ES JOIN TURNIER T ON T.ID = ES.TURNIER_ID WHERE T.ID = '{Id}'";
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
                            {
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Spiel s = new Spiel();
                                        s.SelektionId(reader.GetInt64("ID"), "Einzelspiel");
                                        result.Add(s);
                                    }
                                }
                            }
                        }
                        catch (MySqlException e)
                        {
#if DEBUG
                            Debug.WriteLine(e.Message);
#endif
                        }
                    }
                    break;
            }
            return result;
        }
        private List<Teilnehmer> GetTeilnehmer()
        {
            List<Teilnehmer> result = new List<Teilnehmer>();
            string turnierart = DatabaseHelper.ReturnSingleValue("Bezeichnung", "turnierart", Turnierart).ToString();
            string sql = "";
            switch (turnierart)
            {
                case "Mannschaftsturnier":
                    sql = $"SELECT M.ID as ID FROM TURNIER_MANNSCHAFT TM JOIN TURNIER T ON T.ID = TM.TURNIER_ID JOIN MANNSCHAFT M ON TM.MANNSCHAFT_ID = M.ID WHERE T.ID = '{Id}'";
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
                            {
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Mannschaft m = new Mannschaft();
                                        m.SelektionId(reader.GetInt64("ID"));
                                        result.Add(m);
                                    }
                                }
                            }
                        }
                        catch (MySqlException e)
                        {
#if DEBUG
                            Debug.WriteLine(e.Message);
#endif
                        }
                    }
                    break;
                case "Einzelturnier":
                    sql = $"SELECT P.ID as ID FROM TURNIER_PERSON TP JOIN TURNIER T ON T.ID = TP.TURNIER_ID JOIN PERSON P ON TP.PERSON_ID = P.ID WHERE T.ID = '{Id}'";
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
                            {
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Teilnehmer t = (Teilnehmer)Activator.CreateInstance(DatabaseHelper.GibTyp(reader.GetInt32("ID")));
                                        t.SelektionId(reader.GetInt64("ID"));
                                        result.Add(t);
                                    }
                                }
                            }
                        }
                        catch (MySqlException e)
                        {
#if DEBUG
                            Debug.WriteLine(e.Message);
#endif
                        }
                    }
                    break;
            }
            return result;
        }
        //private void SpeichereSpiele()
        //{
        //    List<Spiel> oldMembers = GetSpiele();

        //    List<Spiel> remove = oldMembers.Except(Spiele).ToList();
        //    List<Spiel> add = Spiele.Except(oldMembers).ToList();

        //    string turnierart = DatabaseHelper.ReturnSingleValue("Bezeichnung", "turnierart", Turnierart).ToString();

        //    switch (turnierart)
        //    {
        //        case "Mannschaftsturnier":
        //            foreach (Spiel s in remove)
        //            {
        //                s.Loeschen();
        //            }
        //            foreach (Spiel s in add)
        //            {
        //                s.Neuanlage("Mannschaftsturnier");
        //            }
        //            break;
        //        case "Einzelturnier":
        //            foreach (Spiel s in remove)
        //            {
        //                s.Loeschen();
        //            }
        //            foreach (Spiel s in add)
        //            {
        //                s.Neuanlage("Einzelturnier");
        //            }
        //            break;
        //    }
        //}
        //public void ErzeugeSpiele(bool mitRueckspiel)
        //{
        //    //n!/(k! * (n-k)!)
        //    for (int i = 0; i < TurnierTeilnehmer.Count; i++)
        //    {
        //        for (int j = i + 1; j < TurnierTeilnehmer.Count; j++)
        //        {
        //            Spiele.Add(new Spiel { Teilnehmer1 = TurnierTeilnehmer[i], Teilnehmer2 = TurnierTeilnehmer[j] });
        //        }
        //    }
        //    if (mitRueckspiel)
        //    {
        //        for (int i = TurnierTeilnehmer.Count - 1; i >= 0; i--)
        //        {
        //            for (int j = i - 1; j >= 0; j--)
        //            {
        //                Spiele.Add(new Spiel { Teilnehmer1 = TurnierTeilnehmer[i], Teilnehmer2 = TurnierTeilnehmer[j] });
        //            }
        //        }
        //    }
        //}
        private void SpeichereTeilnehmer()
        {
            List<Teilnehmer> oldMembers = GetTeilnehmer();

            List<Teilnehmer> remove = oldMembers.Except(TurnierTeilnehmer).ToList();
            List<Teilnehmer> add = TurnierTeilnehmer.Except(oldMembers).ToList();

            string turnierart = DatabaseHelper.ReturnSingleValue("Bezeichnung", "turnierart", Turnierart).ToString();
            string sql = "";

            switch (turnierart)
            {
                case "Mannschaftsturnier":
                    sql = $"DELETE FROM TURNIER_MANNSCHAFT WHERE TURNIER_ID = '{Id}' AND MANNSCHAFT_ID IN ('{string.Join("', '", remove.Select(x => x.Id))}')";
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
                            {
                                cmd.ExecuteNonQuery();
                                foreach (Teilnehmer t in add)
                                {
                                    string insertSql = $"INSERT INTO TURNIER_MANNSCHAFT (MANNSCHAFT_ID, TURNIER_ID) VALUES ({t.Id}, {Id})";
                                    cmd.CommandText = insertSql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (MySqlException e)
                        {
#if DEBUG
                            Debug.WriteLine(e.Message);
#endif
                        }
                    }
                    break;
                case "Einzelturnier":
                    sql = $"DELETE FROM TURNIER_PERSON WHERE TURNIER_ID = '{Id}' AND PERSON_ID IN ('{string.Join("', '", remove.Select(x => x.Id))}')";
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
                            {
                                cmd.ExecuteNonQuery();
                                foreach (Teilnehmer t in add)
                                {
                                    string insertSql = $"INSERT INTO TURNIER_PERSON (PERSON_ID, TURNIER_ID) VALUES ({t.Id}, {Id})";
                                    cmd.CommandText = insertSql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (MySqlException e)
                        {
#if DEBUG
                            Debug.WriteLine(e.Message);
#endif
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}