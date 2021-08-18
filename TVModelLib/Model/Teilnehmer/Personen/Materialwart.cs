using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using TVModelLib.Model.TeilnehmerNS.Personen;

namespace TVModelLib.Model.TeilnehmerNS.Personen
{

    [Serializable]
    public class Materialwart : Person
    {
        #region Attributes
        private int _anzahlBaelle;
        private int _anzahlWasserflaschen;
        #endregion
        #region Properties
        [DisplayMetaInformation("Anzahl Bälle", 26, true, ControlType.ctEditText)]
        public int AnzahlBaelle { get => _anzahlBaelle; set => _anzahlBaelle = value; }
        [DisplayMetaInformation("Anzahl Wasserflaschen", 27, true, ControlType.ctEditText)]
        public int AnzahlWasserflaschen { get => _anzahlWasserflaschen; set => _anzahlWasserflaschen = value; }
        #endregion
        #region Constructors
        public Materialwart() : base()
        {
            AnzahlBaelle = 0;
            AnzahlWasserflaschen = 0;
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
                            string updateSpieler = $"UPDATE MATERIALWART SET ANZAHL_BAELLE = {AnzahlBaelle}, ANZAHL_WASSERFLASCHEN = '{AnzahlWasserflaschen}' WHERE PERSON_ID = '{Id}'";
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
                    string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, M.ANZAHL_BAELLE, M.ANZAHL_WASSERFLASCHEN " +
                        $"FROM PERSON P " +
                        $"JOIN MATERIALWART M " +
                        $"ON P.ID = M.PERSON_ID " +
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
                                AnzahlBaelle = reader.GetInt32("ANZAHL_BAELLE");
                                AnzahlWasserflaschen = reader.GetInt32("ANZAHL_WASSERFLASCHEN");
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
                            string insertSpieler = $"INSERT INTO MATERIALWART (PERSON_ID, ANZAHL_BAELLE, ANZAHL_WASSERFLASCHEN) VALUES ('{cmd.LastInsertedId}', {AnzahlBaelle}, '{AnzahlWasserflaschen}')";
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
