using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;

namespace Turnierverwaltung
{
    public class Fussballspieler : Spieler
    {
        #region Attributes
        private int _anzahlTore;

        #endregion
        #region Properties
        [DisplayName("Anzahl Tore")]
        public int AnzahlTore { get => _anzahlTore; set => _anzahlTore = value; }
        #endregion
        #region Constructors
        public Fussballspieler()
        {

        }
        public Fussballspieler(long id) : base()
        {
            SelektionId(id);
        }
        #endregion
        #region Methods

        public override string GetInformation()
        {
            return base.GetInformation() + $"{AnzahlTore}";
        }

        public override void SelektionId(long id)
        {
            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                Connection.Open();

                string selektionstring = $"SELECT " +
                    $"S.ID, " +
                    $"FS.ANZAHL_TORE, " +
                    $"S.VORNAME, " +
                    $"S.NACHNAME, " +
                    $"M.NAME, " +
                    $"S.GEBURTSTAG " +
                    $"FROM SPIELER S " +
                    $"JOIN FUSSBALLSPIELER FS " +
                    $"ON S.ID = FS.SPIELER_ID " +
                    $"JOIN MANNSCHAFT M " +
                    $"ON M.ID = S.MANNSCHAFT_ID " +
                    $"WHERE S.ID = {id}";

                MySqlCommand command = new MySqlCommand(selektionstring, Connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    AnzahlTore = reader.GetInt32("ANZAHL_TORE");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Mannschaft = reader.GetString("NAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            string updateDetails = $"UPDATE FUSSBALLSPIELER SET ANZAHL_TORE='{AnzahlTore}' WHERE SPIELER_ID='{Id}'";

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
            string insertSpieler = $"INSERT INTO SPIELER (VORNAME, NACHNAME, GEBURTSTAG, MANNSCHAFT_ID) VALUES ('{Vorname}', '{Nachname}', '{Geburtstag}', (SELECT M.ID FROM MANNSCHAFT M WHERE M.NAME='{Mannschaft}'))";


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
                string insertFussballer = $"INSERT INTO FUSSBALLSPIELER (SPIELER_ID, ANZAHL_TORE) VALUES ('{Id}', '{AnzahlTore}')";
                command.CommandText = insertFussballer;
                res = res && (command.ExecuteNonQuery() == 1);
                transaction.Commit();
            }
            catch (Exception e)
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

            //Wird nicht benötigt, da Löschen über FK
            //string deleteFussballer = $"DELETE FROM FUSSBALLSPIELER WHERE SPIELER_ID='{Id}'";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            Connection.Open();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection
            };

            try
            {
                command.CommandText = deleteSpieler;
                res = command.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                res = false;
            }
            finally
            {
                Connection.Close();
            }
            return res;
        }
    }
    #endregion
}

