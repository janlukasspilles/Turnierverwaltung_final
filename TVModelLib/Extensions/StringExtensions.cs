using System;

namespace TVModelLib.Extensions
{
    public static class StringExtensions
    {       
        public static bool AsTrueFalse(this string value)
        {
            switch (value)
            {
                case "Ja":
                    return true;
                case "Nein":
                    return false;
                default: throw new FormatException("Es können nur Strings mit dem Wert Ja oder Nein umgewandelt werden.");
            }
        }             
    }
}