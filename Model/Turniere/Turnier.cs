using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Turnierverwaltung.Model.TeilnehmerNS;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;

namespace Turnierverwaltung_final.Model.Turniere
{
    public class Turnier
    {
        #region Attributes      
        private long _id;
        private List<Spiel> _spiele;
        private int _sportartId;
        private string _turnierName;
        private List<Teilnehmer> _teilnehmer;
        private int _turnierart;
        #endregion
        #region Properties
        public List<Spiel> Spiele { get => _spiele; set => _spiele = value; }
        public int SportartId { get => _sportartId; set => _sportartId = value; }
        public string Turniername { get => _turnierName; set => _turnierName = value; }
        public long Id { get => _id; set => _id = value; }
        public List<Teilnehmer> Teilnehmer { get => _teilnehmer; set => _teilnehmer = value; }
        public int Turnierart { get => _turnierart; set => _turnierart = value; }
        #endregion
        #region Constructors
        public Turnier()
        {

        }
        #endregion
        #region Methods
        public void SelektionId(long id)
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    string selectionString = $"SELECT T.* " +
                        $"FROM TURNIER T " +
                        $"WHERE T.ID = '{id}'";

                    using (MySqlCommand cmd = new MySqlCommand(selectionString, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = reader.GetInt64("ID");
                                SportartId = reader.GetInt32("SPORTART_ID");
                                Turniername = reader.GetString("BEZEICHNUNG");
                                Turnierart = reader.GetInt32("TURNIERART_ID");
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

        private List<Teilnehmer> GetTeilnehmer()
        {
            List<Teilnehmer> result = new List<Teilnehmer>();

            return result;
        }
        #endregion
    }
}