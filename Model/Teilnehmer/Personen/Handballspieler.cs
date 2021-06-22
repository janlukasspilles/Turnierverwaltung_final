using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using Turnierverwaltung_final.Helper;

namespace Turnierverwaltung_final.Model.TeilnehmerNS.Personen
{
    [Serializable]
    public class Handballspieler : Person
    {
        #region Attributes
        private int _tore;
        private string _position;
        #endregion
        #region Properties
        [DisplayMetaInformation("Tore", 7, true, ControlType.ctEditText)]
        public int Tore { get => _tore; set => _tore = value; }
        [DisplayMetaInformation("Position", 8, true, ControlType.ctEditText)]
        public string Position { get => _position; set => _position = value; }
        #endregion
        #region Constructors
        public Handballspieler() : base()
        {

        }
        #endregion
        #region Methods         
        public override bool Speichern()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            con.Open();
            MySqlTransaction trans = con.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand() { Connection = con, Transaction = trans };
                string updatePerson = $"UPDATE PERSON SET VORNAME = '{Vorname}', " +
                    $"NACHNAME = '{Nachname}', " +
                    $"GEBURTSTAG = '{Geburtstag}' " +
                    $"WHERE ID = {Id}";
                cmd.CommandText = updatePerson;
                cmd.ExecuteNonQuery();
                string updateSpieler = $"UPDATE HANDBALLSPIELER SET TORE = {Tore}, POSITION = '{Position}' WHERE PERSON_ID = '{Id}'";
                cmd.CommandText = updateSpieler;
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                res = false;
                trans.Rollback();
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            return res;
        }
        public override void SelektionId(long id)
        {
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();
                string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, HS.TORE, HS.POSITION " +
                    $"FROM PERSON P " +
                    $"JOIN HANDBALLSPIELER HS " +
                    $"ON P.ID = HS.PERSON_ID " +
                    $"WHERE P.ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                    Tore = reader.GetInt32("TORE");
                    Position = reader.GetString("POSITION");
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
        public override bool Neuanlage()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            con.Open();
            MySqlTransaction trans = con.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand() { Connection = con, Transaction = trans };
                string insertPerson = $"INSERT INTO PERSON (VORNAME, NACHNAME, GEBURTSTAG) VALUES ('{Vorname}', '{Nachname}', '{Geburtstag}')";
                cmd.CommandText = insertPerson;
                cmd.ExecuteNonQuery();
                Id = cmd.LastInsertedId;
                string insertSpieler = $"INSERT INTO HANDBALLSPIELER (PERSON_ID, TORE, POSITION) VALUES ('{cmd.LastInsertedId}', {Tore}, '{Position}')";
                cmd.CommandText = insertSpieler;
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                Id = 0;
                res = false;
                trans.Rollback();
            }
            finally
            {
                con.Close();
            }

            return res;
        }
        public override bool Loeschen()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();
                string deleteSql = $"DELETE FROM PERSON WHERE ID = '{Id}'";
                MySqlCommand cmd = new MySqlCommand(deleteSql, con);
                cmd.ExecuteNonQuery();
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
                con.Close();
            }

            return res;
        }
        #endregion
    }
}