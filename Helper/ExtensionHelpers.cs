using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turnierverwaltung_final.Helper
{
    public static class ExtensionHelpers
    {
        public static string AsJaNeinString(this bool value)
        {
            return value ? "Ja" : "Nein";
        }

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

        public static void MoveTo<T>(this List<T> value, T item, List<T> targetList)
        {
            value.Remove(item);
            targetList.Add(item);
        }
    }
}