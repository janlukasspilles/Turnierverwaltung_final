using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVModelLib.Model.Benutzeranmeldung
{
    public class Benutzer
    {
        #region Attributes
        private string _benutzername;
        private string _passwort;
        private int _id;
        private int _rolleId;
        #endregion
        #region Properties
        [DisplayMetaInformation("Benutzername", 27, true, ControlType.ctEditText)]
        public string Benutzername { get => _benutzername; set => _benutzername = value; }
        [DisplayMetaInformation("Passwort", 28, true, ControlType.ctEditText)]
        public string Passwort { get => _passwort; set => _passwort = value; }
        [DisplayMetaInformation("Id", 26, false, ControlType.ctEditText)]
        public int Id { get => _id; set => _id = value; }
        [DisplayMetaInformation("Benutzerrolle", 29, true, ControlType.ctDomain, DdlList.dlSportarten, DomainName = "BENUTZERROLLE")]
        public int RolleId { get => _rolleId; set => _rolleId = value; }
        #endregion
        #region Constructors
        #endregion
        #region Methods
        public bool Speichern()
        {
            bool res = true;
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand() { Connection = con })
                    {
                        string updateBenutzer = $"UPDATE BENUTZER SET BENUTZERNAME = '{Benutzername}', " +
                            $"PASSWORT = '{Passwort}', " +
                            $"ROLLE_ID = '{RolleId}' " +
                            $"WHERE ID = {Id}";
                        cmd.CommandText = updateBenutzer;
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

        public void SelektionId(long id)
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string selectionString = $"SELECT B.* " +
                        $"FROM BENUTZER B " +
                        $"WHERE B.ID = '{id}'";

                    using (MySqlCommand cmd = new MySqlCommand(selectionString, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = reader.GetInt32("ID");
                                Benutzername = reader.GetString("BENUTZERNAME");
                                Passwort = reader.GetString("PASSWORT");
                                RolleId = reader.GetInt32("ROLLE_ID");
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
        public bool Neuanlage()
        {
            bool res = true;
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand() { Connection = con })
                    {
                        string insertPerson = $"INSERT INTO BENUTZER (BENUTZERNAME, PASSWORT, ROLLE_ID) VALUES ('{Benutzername}', '{Passwort}', '{RolleId}')";
                        cmd.CommandText = insertPerson;
                        cmd.ExecuteNonQuery();
                        Id = (int)cmd.LastInsertedId;
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
        public bool Loeschen()
        {
            bool res = true;
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string deleteSql = $"DELETE FROM BENUTZER WHERE ID = '{Id}'";
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
