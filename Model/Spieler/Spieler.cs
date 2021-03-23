using MySql.Data.MySqlClient;
using System;
using Turnierverwaltung_final.Model.Spieler;

namespace Turnierverwaltung
{
    public class Spieler : Person
    {
        #region Attributes      
        private bool _verletzt;
        #endregion
        #region Properties        
        public bool Verletzt { get => _verletzt; set => _verletzt = value; }
        #endregion
        #region Constructors
        public Spieler() : base()
        {

        }
        #endregion
        #region Methods       
        public override bool Speichern()
        {

        }
        public override void SelektionId(long id)
        {
            MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;");
            try
            {
                con.Open();
                string selectionString = $"SELECT P.ID, P.VORNAME, P.NACHNAME, P.GEBURTSTAG, SP.VERLETZT, M.NAME " +
                    $"FROM PERSON P " +
                    $"JOIN personen_mannschaften PM " +
                    $"ON Pm.PERSON_ID = P.ID " +
                    $"JOIN mannschaft M " +
                    $"ON PM.MANNSCHAFT_ID = M.ID " +
                    $"JOIN SPIELER SP " +
                    $"ON P.ID = SP.PERSON_ID " +
                    $"WHERE P.ID = '{id}'";

                MySqlCommand cmd = new MySqlCommand(selectionString, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Id = reader.GetInt64("ID");
                    Vorname = reader.GetString("VORNAME");
                    Nachname = reader.GetString("NACHNAME");
                    Geburtstag = reader.GetDateTime("GEBURTSTAG").ToString("yyyy-MM-dd");
                    Verletzt = reader.GetBoolean("VERLETZT");
                    Mannschaft = reader.GetString("NAME");
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
        }
        public override bool Loeschen()
        {
        }
        #endregion
    }
}
