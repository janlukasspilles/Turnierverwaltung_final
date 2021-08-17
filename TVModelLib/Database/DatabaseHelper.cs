using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;

namespace TVModelLib.Database
{
    public static class DatabaseHelper
    {
        public static object ReturnSingleValue(string fieldname, string tablename, long id)
        {
            string sql = $"SELECT {fieldname} FROM {tablename} WHERE ID = '{id}'";
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        return cmd.ExecuteScalar();
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

        public static object ReturnSingleValue(string sql)
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        return cmd.ExecuteScalar();
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
        public static Type GibTyp(long id)
        {
            string sql = "SELECT case " +
                "when((SELECT 1 FROM TRAINER T WHERE T.PERSON_ID = P.ID) IS NOT NULL) THEN 'Trainer' " +
                "when((SELECT 1 FROM PHYSIO PH WHERE PH.PERSON_ID = P.ID) IS NOT NULL) THEN 'Physio' " +
                "when((SELECT 1 FROM FUSSBALLSPIELER FS WHERE FS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Fussballspieler' " +
                "when((SELECT 1 FROM HANDBALLSPIELER HS WHERE HS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Handballspieler' " +
                "when((SELECT 1 FROM TENNISSPIELER TS WHERE TS.PERSON_ID = P.ID) IS NOT NULL) THEN 'Tennisspieler' " +
                "END AS Profession " +
                "FROM PERSON P " +
                $"WHERE P.ID = {id}";
            string prof = "";
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
            {
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                prof = reader.GetString("PROFESSION");
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
            switch (prof)
            {
                case "Trainer":
                    return Type.GetType($"TVModelLib.Model.TeilnehmerNS.Personen.Trainer");
                case "Physio":
                    return Type.GetType($"TVModelLib.Model.TeilnehmerNS.Personen.Physio");
                case "Fussballspieler":
                    return Type.GetType($"TVModelLib.Model.TeilnehmerNS.Personen.Fussballspieler");
                case "Handballspieler":
                    return Type.GetType($"TVModelLib.Model.TeilnehmerNS.Personen.Handballspieler");
                case "Tennisspieler":
                    return Type.GetType($"TVModelLib.Model.TeilnehmerNS.Personen.Tennisspieler");
                default: throw new Exception("Invalid Type");
            }
        }
    }
}