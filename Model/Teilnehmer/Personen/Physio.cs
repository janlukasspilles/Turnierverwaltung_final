using MySql.Data.MySqlClient;
using System;

namespace Turnierverwaltung_final.Model.TeilnehmerNS.Personen
{
    [Serializable]
    public class Physio : Person
    {
        #region Attributes 
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public Physio() : base()
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
                //string updateSpieler = $"UPDATE SPIELER SET VERLETZT = {Convert.ToInt32(Verletzt)} WHERE PERSON_ID = '{Id}'";
                //cmd.CommandText = updateSpieler;
                //cmd.ExecuteNonQuery();
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
                string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG " +
                    $"FROM PERSON P " +
                    $"JOIN PHYSIO PH " +
                    $"ON P.ID = PH.PERSON_ID " +
                    $"WHERE P.ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
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
                //string insertSpieler = $"INSERT INTO SPIELER (PERSON_ID, VERLETZT) VALUES ('{cmd.LastInsertedId}', '{Convert.ToInt32(Verletzt)}')";
                //cmd.CommandText = insertSpieler;
                //cmd.ExecuteNonQuery();
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
