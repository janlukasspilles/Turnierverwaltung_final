using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung_final.Model.TeilnehmerNS.Personen
{
    [Serializable]
    public class Fussballspieler : Person
    {
        #region Attributes
        private int _tore;
        private string _position;
        #endregion
        #region Properties
        [DisplayMetaInformation("Tore", 5, true, ControlType.ctEditText)]
        public int Tore { get => _tore; set => _tore = value; }
        [DisplayMetaInformation("Position", 6, true, ControlType.ctEditText)]
        public string Position { get => _position; set => _position = value; }
        #endregion
        #region Constructors
        public Fussballspieler() : base()
        {
            Tore = 0;
            Position = "";
        }
        #endregion
        #region Methods         
        public override bool Speichern()
        {
            bool res = true;
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        using (MySqlCommand cmd = new MySqlCommand() { Connection = con, Transaction = trans })
                        {
                            string updatePerson = $"UPDATE PERSON SET VORNAME = '{Vorname}', " +
                                $"NACHNAME = '{Nachname}', " +
                                $"GEBURTSTAG = '{Geburtstag}' " +
                                $"WHERE ID = {Id}";
                            cmd.CommandText = updatePerson;
                            cmd.ExecuteNonQuery();
                            string updateSpieler = $"UPDATE FUSSBALLSPIELER SET TORE = {Tore}, POSITION = '{Position}' WHERE PERSON_ID = '{Id}'";
                            cmd.CommandText = updateSpieler;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                }
                catch (MySqlException e)
                {
                    res = false;
#if DEBUG
                    Debug.WriteLine(e.Message);
#endif
                    throw e;
                }                
            }
            return res;
        }

        public override void SelektionId(long id)
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, FS.TORE, FS.POSITION " +
                        $"FROM PERSON P " +
                        $"JOIN FUSSBALLSPIELER FS " +
                        $"ON P.ID = FS.PERSON_ID " +
                        $"WHERE P.ID = '{id}'";

                    using (MySqlCommand cmd = new MySqlCommand(selectionString, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = reader.GetInt64("ID");
                                Vorname = reader.GetString("VORNAME");
                                Nachname = reader.GetString("NACHNAME");
                                Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                                Tore = reader.GetInt32("TORE");
                                Position = reader.GetString("POSITION");
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
        public override bool Neuanlage()
        {
            bool res = true;
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        using (MySqlCommand cmd = new MySqlCommand() { Connection = con, Transaction = trans })
                        {
                            string insertPerson = $"INSERT INTO PERSON (VORNAME, NACHNAME, GEBURTSTAG) VALUES ('{Vorname}', '{Nachname}', '{Geburtstag}')";
                            cmd.CommandText = insertPerson;
                            cmd.ExecuteNonQuery();
                            Id = cmd.LastInsertedId;
                            string insertSpieler = $"INSERT INTO FUSSBALLSPIELER (PERSON_ID, TORE, POSITION) VALUES ('{cmd.LastInsertedId}', {Tore}, '{Position}')";
                            cmd.CommandText = insertSpieler;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }                    
                }
                catch (MySqlException e)
                {
                    res = false;
                    Id = 0;
#if DEBUG
                    Debug.WriteLine(e.Message);
#endif
                    throw e;
                }
            }
            return res;
        }
        public override bool Loeschen()
        {
            bool res = true;
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string deleteSql = $"DELETE FROM PERSON WHERE ID = '{Id}'";
                    using (MySqlCommand cmd = new MySqlCommand(deleteSql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException e)
                {
                    res = false;
#if DEBUG
                    Debug.WriteLine(e.Message);
#endif
                    throw e;
                }
            }
            return res;
        }
        #endregion
    }
}