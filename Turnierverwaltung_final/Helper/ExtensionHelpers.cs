using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

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

        public static Control FindControlRecursive(this Control value, string name)
        {
            if (value != null && value.ID == name)
                return value;
            foreach (Control c in value.Controls)
            {
                Control tmp = FindControlRecursive(c, name);
                if (tmp != null)
                    return tmp;
            }
            return null;
        }
    }
}