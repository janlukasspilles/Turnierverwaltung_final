namespace Turnierverwaltung.Helper
{
    public static class GlobalConstants
    {
        public const string connectionString = "Server=127.0.0.1;Database=turnierverwaltung;Uid=user;Pwd=user;";
        public const string resourcePath = "Turnierverwaltung.Resources";
        public const string resourceSQLPath = resourcePath + ".SQL";
        public const string resourceSQLStructurePath = resourceSQLPath + ".Structure";
    }

    public enum DdlList
    {
        dlSportarten,
        dlTurnierarten
    }
    
    public enum ControlType
    {
        ctEditText,
        ctDomain,
        ctCheck,
        ctDate
    }
}