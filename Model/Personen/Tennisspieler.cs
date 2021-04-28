using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Turnierverwaltung_final.Model.Spieler;

namespace Turnierverwaltung_final.Model.Personen
{
    public class Tennisspieler : Person
    {
        #region Attributes
        private int _anzahlSpiele;
        private int _anzahlGewonnen;
        #endregion
        #region Properties
        [Display(Name = "Anzahl Spiele", Order = 9)]
        public int AnzahlSpiele { get => _anzahlSpiele; set => _anzahlSpiele = value; }
        [Display(Name = "Anzahl Gewonnene Spiele", Order = 10)]
        public int AnzahlGewonnen { get => _anzahlGewonnen; set => _anzahlGewonnen = value; }
        #endregion
        #region Constructors
        public Tennisspieler() : base()
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
                string updateSpieler = $"UPDATE TENNISSPIELER SET ANZAHL_SPIELE = {AnzahlSpiele}, ANZAHL_GEWONNEN = '{AnzahlGewonnen}' WHERE PERSON_ID = '{Id}'";
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
                string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, TS.ANZAHL_SPIELE, TS.ANZAHL_GEWONNEN" +
                    $"FROM PERSON P " +
                    $"JOIN TENNISSPIELER TS " +
                    $"ON P.ID = TS.PERSON_ID " +
                    $"WHERE P.ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                    AnzahlSpiele = reader.GetInt32("ANZAHL_SPIELE");
                    AnzahlGewonnen = reader.GetInt32("ANZAHL_GEWONNEN");
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
                string insertSpieler = $"INSERT INTO TENNISSPIELER (PERSON_ID, ANZAHL_SPIELE, ANZAHL_GEWONNEN) VALUES ('{cmd.LastInsertedId}', {AnzahlSpiele}, {AnzahlGewonnen})";
                cmd.CommandText = insertSpieler;
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