using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Reflection;

namespace Turnierverwaltung.Helper
{
    public static class DatabaseCreator
    {
        public static void GenerateDatabase()
        {
            using (MySqlConnection con = new MySqlConnection(GlobalConstants.connectionString))
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
                        throw e;
                    }
            }
        }
        private static string CreateDatabase()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream($"{GlobalConstants.resourceSQLStructurePath}.CreateDatabase.sql"))
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
            using (Stream s = assembly.GetManifestResourceStream($"{GlobalConstants.resourceSQLStructurePath}.CreateTables.sql"))
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
            using (Stream s = assembly.GetManifestResourceStream($"{GlobalConstants.resourceSQLStructurePath}.CreateTriggers.sql"))
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
            using (Stream s = assembly.GetManifestResourceStream($"{GlobalConstants.resourceSQLStructurePath}.CreateProcedures.sql"))
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
            using (Stream s = assembly.GetManifestResourceStream($"{GlobalConstants.resourceSQLStructurePath}.CreateViews.sql"))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

        private static string InsertExampleData()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream($"{GlobalConstants.resourceSQLPath}.ExampleData.sql"))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
