using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Turnierverwaltung
{
    public class Mannschaft : Teilnehmer
    {
        #region Attributes
        private List<Teilnehmer> _mitglieder;
        private string _stadt;
        private string _gruendungsjahr;
        #endregion
        #region Properties
        public List<Teilnehmer> Mitglieder { get => _mitglieder; set => _mitglieder = value; }
        public string Stadt { get => _stadt; set => _stadt = value; }
        public string Gruendungsjahr { get => _gruendungsjahr; set => _gruendungsjahr = value; }
        #endregion
        #region Constructors
        public Mannschaft()
        {
            Mitglieder = new List<Teilnehmer>();
        }
        #endregion
        #region Methods

        public bool NeuesMannschaftsMitglied(Teilnehmer teilnehmer)
        {
            //teilnehmer.Mannschaft = Name;
            //if (teilnehmer.Speichern())
            //{
            //    Mitglieder.Add(teilnehmer);
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        public Teilnehmer MitgliedVerlaesstMannschaft(string name)
        {
            foreach (Teilnehmer t in Mitglieder)
            {
                if (t.Name == name)
                {
                    Teilnehmer res = t;
                    Mitglieder.Remove(t);
                    return res;
                }
                else
                {
                    //Nichts
                }
            }
            throw new Exception("Kein Mitglied dieses Teams hat diesen Namen!");
        }

        public override bool Speichern()
        {
            bool res = true;
            string updateMannschaft = $"UPDATE MANNSCHAFT SET NAME='{Name}', STADT='{Stadt}', GRUENDUNGSJAHR='{Gruendungsjahr}' WHERE ID='{Id}'";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            Connection.Open();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection
            };

            try
            {
                command.CommandText = updateMannschaft;
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

        public override void SelektionId(long id)
        {
            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            try
            {
                Connection.Open();

                string selektionstring = $"SELECT * FROM MANNSCHAFT WHERE ID = '{id}'";
                MySqlCommand command = new MySqlCommand(selektionstring, Connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Name = reader.GetString("NAME");
                    Stadt = reader.GetString("STADT");
                    Gruendungsjahr = reader.GetDateTime("GRUENDUNGSJAHR").ToString("yyyy-MM-dd");
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

        public bool SelektiereMitgliederInListe()
        {
            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            try
            {
                Connection.Open();

                string selektionstring = $"SELECT ID FROM SPIELER WHERE MANNSCHAFT_ID = '{Id}'";
                MySqlCommand command = new MySqlCommand(selektionstring, Connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //Mitglieder.Add(new Teilnehmer().SelektionId(reader.GetInt64("ID")));
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
            return true;
        }       

        public override bool Neuanlage()
        {
            bool res = true;
            string insertMannschaft = $"INSERT INTO MANNSCHAFT (NAME, STADT, GRUENDUNGSJAHR) VALUES ('{Name}', '{Stadt}', '{Gruendungsjahr}')";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
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

        public override bool Loeschen()
        {
            bool res = true;
            string deleteMannschaft = $"DELETE FROM MANNSCHAFT WHERE ID='{Id}'";

            MySqlConnection Connection = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung2;Uid=user;Pwd=user;");
            Connection.Open();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection
            };

            try
            {
                command.CommandText = deleteMannschaft;
                res = command.ExecuteNonQuery() == 1;
                Mitglieder.Clear();
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
        #endregion
    }
}
