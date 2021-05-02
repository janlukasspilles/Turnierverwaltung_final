using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
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
        [DisplayMetaInformation("Tore", 5, true, ControlType.ctEdit)]
        public int Tore { get => _tore; set => _tore = value; }
        [DisplayMetaInformation("Position", 6, true, ControlType.ctEdit)]
        public string Position { get => _position; set => _position = value; }
        #endregion
        #region Constructors
        public Fussballspieler() : base()
        {

        }
        #endregion
        #region Methods         
        public override bool Speichern()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
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
                string updateSpieler = $"UPDATE FUSSBALLSPIELER SET TORE = {Tore}, POSITION = '{Position}' WHERE PERSON_ID = '{Id}'";
                cmd.CommandText = updateSpieler;
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                res = false;
                trans.Rollback();
            }
            finally
            {
                con.Close();
            }

            return res;
        }
        public override void SelektionId(long id)
        {
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();
                string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, FS.TORE, FS.POSITION " +
                    $"FROM PERSON P " +
                    $"JOIN FUSSBALLSPIELER FS " +
                    $"ON P.ID = FS.PERSON_ID " +
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
            }
            finally
            {
                con.Close();
            }
        }
        public override bool Neuanlage()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            con.Open();
            MySqlTransaction trans = con.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand() { Connection = con, Transaction = trans };
                string insertPerson = $"INSERT INTO PERSON (VORNAME, NACHNAME, GEBURTSTAG) VALUES ('{Vorname}', '{Nachname}', '{Geburtstag}')";
                cmd.CommandText = insertPerson;
                cmd.ExecuteNonQuery();
                Id = cmd.LastInsertedId;
                string insertSpieler = $"INSERT INTO FUSSBALLSPIELER (PERSON_ID, TORE, POSITION) VALUES ('{cmd.LastInsertedId}', {Tore}, '{Position}')";
                cmd.CommandText = insertSpieler;
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                res = false;
                Id = 0;
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
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();
                string deleteSql = $"DELETE FROM PERSON WHERE ID = '{Id}'";
                MySqlCommand cmd = new MySqlCommand(deleteSql, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
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