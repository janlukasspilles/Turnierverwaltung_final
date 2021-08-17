using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using TVModelLib;

namespace TVModelLib.Model
{
    public class Sportart
    {
        #region Attributes
        private long _id;
        private string _bezeichnung;
        private int _anzahlPunkteSieg;
        private int _anzahlPunkteVerlust;
        private int _anzahlPunktePatt;
        private int _anzahlSpielerProTeam;
        #endregion
        #region Properties
        public long Id { get => _id; set => _id = value; }
        public string Bezeichnung { get => _bezeichnung; set => _bezeichnung = value; }
        public int AnzahlPunkteSieg { get => _anzahlPunkteSieg; set => _anzahlPunkteSieg = value; }
        public int AnzahlPunkteVerlust { get => _anzahlPunkteVerlust; set => _anzahlPunkteVerlust = value; }
        public int AnzahlPunktePatt { get => _anzahlPunktePatt; set => _anzahlPunktePatt = value; }
        public int AnzahlSpielerProTeam { get => _anzahlSpielerProTeam; set => _anzahlSpielerProTeam = value; }
        #endregion
        #region Constructors       
        public Sportart()
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
                string updateSportart = $"UPDATE SPORTART SET BEZEICHNUNG = '{Bezeichnung}', " +
                    $"ANZAHL_PUNKTE_SIEG = {AnzahlPunkteSieg}, " +
                    $"ANZAHL_PUNKTE_VERLUST = {AnzahlPunkteVerlust}, " +
                    $"ANZAHL_PUNKTE_PATT = {AnzahlPunktePatt}, " +
                    $"ANZAHL_SPIELER_PRO_TEAM = {AnzahlSpielerProTeam} " +
                    $"WHERE ID = {Id}";
                cmd.CommandText = updateSportart;
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
                    $"FROM SPORTART " +
                    $"WHERE ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Bezeichnung = reader.GetString("BEZEICHNUNG");
                    AnzahlPunkteSieg = reader.GetInt32("ANZAHL_PUNKTE_SIEG");
                    AnzahlPunkteVerlust = reader.GetInt32("ANZAHL_PUNKTE_VERLUST");
                    AnzahlPunktePatt = reader.GetInt32("ANZAHL_PUNKTE_PATT");
                    AnzahlSpielerProTeam = reader.GetInt32("ANZAHL_SPIELER_PRO_TEAM");
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
        public bool Neuanlage()
        {
            bool res = true;
            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            con.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand() { Connection = con };
                string insertSportart = $"INSERT INTO SPORTART (BEZEICHNUNG, ANZAHL_PUNKTE_SIEG, ANZAHL_PUNKTE_VERLUST, ANZAHL_PUNKTE_PATT, ANZAHL_SPIELER_PRO_TEAM) VALUES ('{Bezeichnung}', {AnzahlPunkteSieg}, {AnzahlPunkteVerlust}, {AnzahlPunktePatt}, {AnzahlSpielerProTeam})";
                cmd.CommandText = insertSportart;
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
                string deleteSql = $"DELETE FROM SPORTART WHERE ID = '{Id}'";
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