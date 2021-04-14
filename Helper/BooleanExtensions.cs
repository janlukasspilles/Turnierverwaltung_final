using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turnierverwaltung_final.Helper
{
    public static class BooleanExtensions
    {
        public static string GetJaNeinString(this bool value)
        {
            return value ? "Ja" : "Nein";
        }
    }
}