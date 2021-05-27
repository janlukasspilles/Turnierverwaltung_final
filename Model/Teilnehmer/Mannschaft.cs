using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung_final.Helper;
using Turnierverwaltung_final.Helper.TurnierverwaltungTypes;
using Turnierverwaltung_final.Model.TeilnehmerNS.Personen;

namespace Turnierverwaltung.Model.TeilnehmerNS
{
    public class Mannschaft : Teilnehmer
    {
        #region Attributes
        private List<Person> _mitglieder;
        private string _stadt;
        private string _gruendungsjahr;
        private int _sportart;
        #endregion
        #region Properties
        public List<Person> Mitglieder { get => _mitglieder; set => _mitglieder = value; }
        [DisplayMetaInformation("Stadt", 13, true, ControlType.ctEditText)]
        public string Stadt { get => _stadt; set => _stadt = value; }
        [DisplayMetaInformation("Gründungsjahr", 14, true, ControlType.ctEditText)]
        public string Gruendungsjahr { get => _gruendungsjahr; set => _gruendungsjahr = value; }
        [DisplayMetaInformation("Sportart", 15, true, ControlType.ctDomain, DdlList.dlSportarten)]
        public int Sportart { get => _sportart; set => _sportart = value; }
        #endregion
        #region Constructors
        public Mannschaft()
        {
            Mitglieder = new List<Person>();
        }
        #endregion
        #region Methods       
        private List<Person> GetMitglieder()
        {
            List<Person> result = new List<Person>();
            Person p = null;
            string sql = "SELECT P.ID," +
                "case " +
                "when((SELECT 1 FROM TRAINER T WHERE T.PERSON_ID = P.ID) IS NOT NULL) THEN 'Trainer' " +
                "when((SELECT 1 FROM PHYSIO PH WHERE PH.PERSON_ID = P.ID) IS NOT NULL) THEN 'Physio' " +
                "when((SELECT 1 FROM FUSSBALLSPIELER FS WHERE FS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Fussballspieler' " +
                "when((SELECT 1 FROM HANDBALLSPIELER HS WHERE HS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Handballspieler' " +
                "when((SELECT 1 FROM TENNISSPIELER TS WHERE TS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Tennisspieler' " +
                "END AS Profession " +
                "FROM PERSON P " +
                "JOIN PERSONEN_MANNSCHAFTEN PM " +
                "ON P.ID = PM.PERSON_ID " +
                "JOIN MANNSCHAFT M " +
                "ON PM.MANNSCHAFT_ID = M.ID " +
                $"WHERE M.ID = {Id}";

            MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString);
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    while (reader.Read())
                    {
                        switch (reader.GetString("Profession"))
                        {
                            case "Trainer":
                                p = new Trainer();
                                break;
                            case "Physio":
                                p = new Physio();
                                break;
                            case "Fussballspieler":
                                p = new Fussballspieler();
                                break;
                            case "Handballspieler":
                                p = new Handballspieler();
                                break;
                            case "Tennisspieler":
                                p = new Tennisspieler();
                                break;
                        }
                        p.SelektionId(reader.GetInt64("ID"));
                        result.Add(p);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public override bool Speichern()
        {
            bool res = true;
            string updateMannschaft = $"UPDATE MANNSCHAFT SET NAME='{Name}', STADT='{Stadt}', GRUENDUNGSJAHR='{Gruendungsjahr}', SPORTART_ID={Sportart} WHERE ID='{Id}'";

            MySqlConnection Connection = new MySqlConnection(GlobalConstants.connectionString);
            Connection.Open();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection
            };

            try
            {
                command.CommandText = updateMannschaft;
                res = command.ExecuteNonQuery() == 1;
                if(Mitglieder != null)
                    SpeicherMitglieder();
            }
            catch (Exception e)
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
            MySqlConnection Connection = new MySqlConnection(GlobalConstants.connectionString);
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
                    Sportart = reader.GetInt32("SPORTART_ID");
                }
                reader.Close();
                Mitglieder = GetMitglieder();
            }
            catch (Exception)
            {
            }
            finally
            {
                Connection.Close();
            }
        }
        
        public override bool Neuanlage()
        {
            bool res = true;
            string insertMannschaft = $"INSERT INTO MANNSCHAFT (NAME, STADT, GRUENDUNGSJAHR, SPORTART_ID) VALUES ('{Name}', '{Stadt}', '{Gruendungsjahr}', '{Sportart}')";

            MySqlConnection Connection = new MySqlConnection(GlobalConstants.connectionString);
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
            catch (Exception e)
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

            MySqlConnection Connection = new MySqlConnection(GlobalConstants.connectionString);
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

        private void SpeicherMitglieder()
        {
            List<Person> oldMembers = GetMitglieder();
            //Löschen
            List<Person> remove = oldMembers.Except(Mitglieder).ToList();
            List<Person> add = Mitglieder.Except(oldMembers).ToList();
            string deleteSql = $"DELETE FROM PERSONEN_MANNSCHAFTEN WHERE MANNSCHAFT_ID = '{Id}' AND PERSON_ID IN('{string.Join("', '", remove.Select(x => x.Id))}')";
            

            MySqlConnection Connection = new MySqlConnection(GlobalConstants.connectionString);
            Connection.Open();

            MySqlCommand command = new MySqlCommand
            {
                Connection = Connection
            };

            try
            {
                command.CommandText = deleteSql;
                command.ExecuteNonQuery();
                foreach(Person p in add)
                {
                    string insertSql = $"INSERT INTO PERSONEN_MANNSCHAFTEN (MANNSCHAFT_ID, PERSON_ID) VALUES ({Id}, {p.Id})";
                    command.CommandText = insertSql;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
                Connection.Close();
            }
        }
        #endregion
    }
}
