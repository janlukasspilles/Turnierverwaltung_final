using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using TVModeLib.Model.TeilnehmerNS;
using TVModelLib;
using TVModelLib.Database;

namespace TVModeLib.Model.TurniereNS
{
    public class Spiel
    {
        #region Attributes
        private long _id;
        private int _turnierId;
        private int _punkteTeilnehmer1;
        private int _punkteTeilnehmer2;
        private Teilnehmer _teilnehmer1;
        private Teilnehmer _teilnehmer2;
        #endregion
        #region Properties
        [DisplayMetaInformation("Punkte Teilnehmer 1", 23, true, ControlType.ctEditText)]
        public int PunkteTeilnehmer1 { get => _punkteTeilnehmer1; set => _punkteTeilnehmer1 = value; }
        [DisplayMetaInformation("Punkte Teilnehmer 2", 25, true, ControlType.ctEditText)]
        public int PunkteTeilnehmer2 { get => _punkteTeilnehmer2; set => _punkteTeilnehmer2 = value; }
        public long Id { get => _id; set => _id = value; }
        public int TurnierId { get => _turnierId; set => _turnierId = value; }
        [DisplayMetaInformation("Teilnehmer 1", 22, false, ControlType.ctEditText)]
        public Teilnehmer Teilnehmer1 { get => _teilnehmer1; set => _teilnehmer1 = value; }
        [DisplayMetaInformation("Teilnehmer 2", 24, false, ControlType.ctEditText)]
        public Teilnehmer Teilnehmer2 { get => _teilnehmer2; set => _teilnehmer2 = value; }
        #endregion
        #region Constructors
        public Spiel()
        {

        }
        #endregion
        #region Methods
        public void Speichern()
        {

        }
        public void Neuanlage(string turnierart)
        {
            string sql = "";
            switch (turnierart)
            {
                case "Mannschaftsturnier":
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        try
                        {
                            con.Open();
                            sql = $"INSERT INTO MANNSCHAFTSSPIEL (MANNSCHAFT1_ID, MANNSCHAFT2_ID, PUNKTE_MANNSCHAFT1, PUNKTE_MANNSCHAFT2, TURNIER_ID) VALUES ('{Teilnehmer1.Id}', '{Teilnehmer2.Id}', '{PunkteTeilnehmer1}', '{PunkteTeilnehmer2}', '{TurnierId}')";
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
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
                    break;
                case "Einzelturnier":
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        try
                        {
                            con.Open();
                            sql = $"INSERT INTO EINZELSPIEL (PERSON1_ID, PERSON2_ID, PUNKTE_PERSON1, PUNKTE_PERSON2, TURNIER_ID) VALUES ('{Teilnehmer1.Id}', '{Teilnehmer2.Id}', '{PunkteTeilnehmer1}', '{PunkteTeilnehmer2}', '{TurnierId}')";
                            using (MySqlCommand cmd = new MySqlCommand(sql, con))
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
                    break;
            }
        }

        public void Loeschen()
        {
            string turnierart = DatabaseHelper.ReturnSingleValue($"SELECT * FROM TURNIER T JOIN TURNIERART TA ON TA.ID = T.TURNIERART_ID WHERE T.ID = '{Id}'").ToString();
            switch (turnierart)
            {
                case "Mannschaftsturnier":
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        try
                        {
                            con.Open();
                            string deleteSql = $"DELETE FROM MANNSCHAFTSSPIEL WHERE ID = '{Id}'";
                            using (MySqlCommand cmd = new MySqlCommand(deleteSql, con))
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
                    break;
                case "Einzelturnier":
                    using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
                    {
                        try
                        {
                            con.Open();
                            string deleteSql = $"DELETE FROM EINZELSPIEL WHERE ID = '{Id}'";
                            using (MySqlCommand cmd = new MySqlCommand(deleteSql, con))
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
                    break;
            }
        }
        public void SelektionId(long id, string turnierart)
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string from = turnierart == "Mannschaftsspiel" ? "MANNSCHAFTSSPIEL" : turnierart == "Einzelspiel" ? "EINZELSPIEL" : "";
                    string selectionString = $"SELECT S.* " +
                            $"FROM {from} S " +
                            $"WHERE S.ID = '{id}'";

                    using (MySqlCommand cmd = new MySqlCommand(selectionString, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = reader.GetInt64("ID");
                                TurnierId = reader.GetInt32("TURNIER_ID");
                                switch (turnierart)
                                {
                                    case "Mannschaftsspiel":
                                        PunkteTeilnehmer1 = reader.GetInt32("PUNKTE_MANNSCHAFT1");
                                        PunkteTeilnehmer2 = reader.GetInt32("PUNKTE_MANNSCHAFT1");
                                        Teilnehmer1 = new Mannschaft();
                                        Teilnehmer1.SelektionId(reader.GetInt64("MANNSCHAFT1_ID"));
                                        Teilnehmer2 = new Mannschaft();
                                        Teilnehmer2.SelektionId(reader.GetInt64("MANNSCHAFT2_ID"));
                                        break;
                                    case "Einzelspiel":
                                        PunkteTeilnehmer1 = reader.GetInt32("PUNKTE_PERSON1");
                                        PunkteTeilnehmer2 = reader.GetInt32("PUNKTE_PERSON2");
                                        Teilnehmer1 = (Teilnehmer)Activator.CreateInstance(DatabaseHelper.GibTyp(reader.GetInt32("PERSON1_ID")));
                                        Teilnehmer1.SelektionId(reader.GetInt64("PERSON1_ID"));
                                        Teilnehmer2 = (Teilnehmer)Activator.CreateInstance(DatabaseHelper.GibTyp(reader.GetInt32("PERSON2_ID")));
                                        Teilnehmer2.SelektionId(reader.GetInt64("PERSON2_ID"));
                                        break;
                                }
                            }
                        }
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
        #endregion
    }
}