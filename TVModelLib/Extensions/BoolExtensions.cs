namespace TVModelLib.Extensions
{
    public static class BoolExtensions
    {
        public static string AsJaNeinString(this bool value)
        {
            return value ? "Ja" : "Nein";
        }
    }
}
