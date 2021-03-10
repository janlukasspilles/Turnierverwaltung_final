using MySql.Data.MySqlClient;
using System;

namespace Turnierverwaltung
{
    public class Tennisspieler : Spieler
    {
        #region Attributes
        private int _anzahlGewonneneSpiele;
        private int _anzahlSpiele;
        #endregion
        #region Properties
        public int AnzahlGewonneneSpiele { get => _anzahlGewonneneSpiele; set => _anzahlGewonneneSpiele = value; }
        public int AnzahlSpiele { get => _anzahlSpiele; set => _anzahlSpiele = value; }
        #endregion
        #region Constructors
        public Tennisspieler() : base()
        {
        }

        public Tennisspieler(long id) : base()
        {
            Id = id;
        }
        #endregion
        #region Methods
        public override string GetInformation()
        {
            return base.GetInformation();
        }

        public override void SelektionId(long id)
        {
            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            try
            {
                Connection.Open();

                string selektionstring = $"SELECT S.ID, " +
                    $"TS.ANZAHL_GEWONNENE_SPIELE, " +
                    $"TS.ANZAHL_SPIELE, " +
                    $"S.VORNAME, " +
                    $"S.NACHNAME, " +
                    $"M.NAME, " +
                    $"S.GEBURTSTAG " +
                    $"FROM SPIELER S " +
                    $"JOIN HANDBALLER_DETAILS TS " +
                    $"ON S.ID = TD.SPIELER_ID " +
                    $"JOIN MANNSCHAFT M " +
                    $"ON M.ID = S.MANNSCHAFT_ID " +
                    $"WHERE S.ID = {id}";
                MySqlCommand command = new MySqlCommand(selektionstring, Connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    AnzahlGewonneneSpiele = reader.GetInt32("ANZAHL_GEWONNENE_SPIELE");
                    AnzahlSpiele = reader.GetInt32("ANZAHL_SPIELE");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Mannschaft = reader.GetString("NAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                Connection.Close();
            }
        }

        public override bool Speichern()
        {
            bool res = true;
            string updateSpieler = $"UPDATE SPIELER SET VORNAME='{Vorname}', NACHNAME='{Nachname}', GEBURTSTAG='{Geburtstag}', MANNSCHAFT_ID=(SELECT M.ID FROM MANNSCHAFT M WHERE M.NAME='{Mannschaft}') WHERE ID='{Id}'";
            string updateDetails = $"UPDATE TENNISSPIELER SET ANZAHL_GEWONNENE_SPIELE='{AnzahlGewonneneSpiele}', ANZAHL_SPIELE='{AnzahlSpiele}' WHERE SPIELER_ID='{Id}'";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            Connection.Open();
            //Transaction, da immer beide Tabellen ein Update benötigen. Wenn ein Update schief geht soll Rollback ausgeführt werden.
            MySqlTransaction transaction = Connection.BeginTransaction();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection,
                Transaction = transaction
            };


            try
            {
                command.CommandText = updateSpieler;
                res = command.ExecuteNonQuery() == 1;
                command.CommandText = updateDetails;
                res = res && (command.ExecuteNonQuery() == 1);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                res = false;
            }
            finally
            {
                Connection.Close();
            }
            return res;
        }

        public override bool Neuanlage()
        {
            bool res = true;
            string insertSpieler = $"INSERT INTO SPIELER (VORNAME, NACHNAME, GEBURTSTAG, MANNSCHAFT_ID) VALUES ('{Vorname}', '{Nachname}', '{Geburtstag}', MANNSCHAFT_ID=(SELECT M.ID FROM MANNSCHAFT M WHERE M.NAME='{Mannschaft}'))";
            string insertTennisspieler = $"INSERT INTO TENNISSPIELER (SPIELER_ID, ANZAHL_SPIELE, GEWONNENE_SPIELE) VALUES ('{Id}', '{AnzahlSpiele}', '{AnzahlGewonneneSpiele}')";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            Connection.Open();
            //Transaction, da immer beide Tabellen ein Update benötigen. Wenn ein Update schief geht soll Rollback ausgeführt werden.
            MySqlTransaction transaction = Connection.BeginTransaction();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection,
                Transaction = transaction
            };

            try
            {
                command.CommandText = insertSpieler;
                res = command.ExecuteNonQuery() == 1;
                Id = command.LastInsertedId;
                command.CommandText = insertTennisspieler;
                res = res && (command.ExecuteNonQuery() == 1);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                res = false;
            }
            finally
            {
                Connection.Close();
            }
            return res;
        }

        public override bool Loeschen()
        {
            bool res = true;
            string deleteSpieler = $"DELETE FROM SPIELER WHERE ID='{Id}'";
            string deleteTennisspieler = $"DELETE FROM TENNISSPIELER WHERE SPIELER_ID='{Id}'";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            Connection.Open();
            //Transaction, da immer beide Tabellen ein Update benötigen. Wenn ein Update schief geht soll Rollback ausgeführt werden.
            MySqlTransaction transaction = Connection.BeginTransaction();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection,
                Transaction = transaction
            };

            try
            {
                command.CommandText = deleteSpieler;
                res = command.ExecuteNonQuery() == 1;
                Id = command.LastInsertedId;
                command.CommandText = deleteTennisspieler;
                res = res && (command.ExecuteNonQuery() == 1);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                res = false;
            }
            finally
            {
                Connection.Close();
            }
            return res;
        }
        #endregion
    }
}
