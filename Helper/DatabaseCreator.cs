using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Turnierverwaltung_final.Helper
{
    public static class DatabaseCreator
    {
        private const string _connectionString = "Server=127.0.0.1;Uid=user;Pwd=user;";
        public static void GenerateDatabase()
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                using (MySqlTransaction trans = con.BeginTransaction())
                using (MySqlCommand cmd = new MySqlCommand() { Connection = con, Transaction = trans })
                    try
                    {
                        string currentStatement = CreateDatabase();
                        if (currentStatement != "")
                        {
                            cmd.CommandText = currentStatement;
                            cmd.ExecuteNonQuery();
                        }
                        currentStatement = CreateTables();
                        if (currentStatement != "")
                        {
                            cmd.CommandText = currentStatement;
                            cmd.ExecuteNonQuery();
                        }
                        currentStatement = CreateTriggers();
                        if (currentStatement != "")
                        {
                            cmd.CommandText = currentStatement;
                            cmd.ExecuteNonQuery();
                        }
                        currentStatement = CreateProcedures();
                        if (currentStatement != "")
                        {
                            cmd.CommandText = currentStatement;
                            cmd.ExecuteNonQuery();
                        }
                        currentStatement = CreateViews();
                        if (currentStatement != "")
                        {
                            cmd.CommandText = currentStatement;
                            cmd.ExecuteNonQuery();
                        }
                        currentStatement = InsertExampleData();
                        if (currentStatement != "")
                        {
                            cmd.CommandText = currentStatement;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                    }
            }
        }
        private static string CreateDatabase()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("Turnierverwaltung_final.Ressources.SQL.Structure.CreateDatabase.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        private static string CreateTables()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("Turnierverwaltung_final.Ressources.SQL.Structure.CreateTables.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        private static string CreateTriggers()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("Turnierverwaltung_final.Ressources.SQL.Structure.CreateTriggers.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        private static string CreateProcedures()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("Turnierverwaltung_final.Ressources.SQL.Structure.CreateProcedures.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        private static string CreateViews()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("Turnierverwaltung_final.Ressources.SQL.Structure.CreateViews.sql"))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

        private static string InsertExampleData()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream("Turnierverwaltung_final.Ressources.SQL.ExampleData.inserts.sql"))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
