using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using TVModelLib;

namespace TVModelLib.Model.TurniereNS
{
    public class Turnierart
    {
        #region Attributes
        private long _id;
        private string _bezeichnung;
        #endregion
        #region Properties
        public long Id { get => _id; set => _id = value; }
        public string Bezeichnung { get => _bezeichnung; set => _bezeichnung = value; }
        #endregion
        #region Constructors       
        public Turnierart()
        {

        }
        #endregion
        #region Methods
        public bool Speichern()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            con.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand() { Connection = con };
                string updateTurnierart = $"UPDATE TURNIERART SET BEZEICHNUNG = '{Bezeichnung}' " +
                    $"WHERE ID = {Id}";
                cmd.CommandText = updateTurnierart;
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
        public override string ToString()
        {
            return Bezeichnung;
        }
        public void SelektionId(long id)
        {
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();
                string selectionString = $"SELECT * " +
                    $"FROM TURNIERART " +
                    $"WHERE ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Bezeichnung = reader.GetString("BEZEICHNUNG");
                }
                reader.Close();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
            }
            finally
            {
                con.Close();
            }
        }
        public bool Neuanlage()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            con.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand() { Connection = con };
                string insertTurnierart = $"INSERT INTO TURNIERART (BEZEICHNUNG) VALUES ('{Bezeichnung}')";
                cmd.CommandText = insertTurnierart;
                cmd.ExecuteNonQuery();
                Id = cmd.LastInsertedId;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                res = false;
                Id = 0;
            }
            finally
            {
                con.Close();
            }

            return res;
        }
        public bool Loeschen()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();
                string deleteSql = $"DELETE FROM TURNIERART WHERE ID = '{Id}'";
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