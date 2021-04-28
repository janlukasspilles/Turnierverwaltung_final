using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;

namespace Turnierverwaltung_final.Model.Personen
{
    public class Trainer : Person
    {
        #region Attributes
        private int _jahreErfahrung;
        private string _lizenz;
        #endregion
        #region Properties
        [Display(Name = "Jahre an Erfahrung", Order = 11)]
        public int JahreErfahrung { get => _jahreErfahrung; set => _jahreErfahrung = value; }
        [Display(Name = "Lizenz", Order = 12)]
        public string Lizenz { get => _lizenz; set => _lizenz = value; }
        #endregion
        #region Constructors
        public Trainer() : base()
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
                string updateTrainer = $"UPDATE TRAINER SET ERFAHRUNG = {JahreErfahrung}, LIZENZ = '{Lizenz}' WHERE PERSON_ID = '{Id}'";
                cmd.CommandText = updateTrainer;
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
                string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, T.ERFAHRUNG, T.LIZENZ " +
                    $"FROM PERSON P " +
                    $"JOIN TRAINER T " +
                    $"ON P.ID = T.PERSON_ID " +
                    $"WHERE P.ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                    JahreErfahrung = reader.GetInt32("ERFAHRUNG");
                    Lizenz = reader.GetString("Lizenz");
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
                string insertSpieler = $"INSERT INTO TRAINER (PERSON_ID, ERFAHRUNG, LIZENZ) VALUES ('{cmd.LastInsertedId}', '{JahreErfahrung}', '{Lizenz}')";
                cmd.CommandText = insertSpieler;
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
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
