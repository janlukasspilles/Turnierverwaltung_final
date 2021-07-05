using System;

namespace TVModelLib.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetTvType(string name) 
            => Type.GetType($"TVModeLib.Model.TeilnehmerNS.Personen.{name}");
    }
}
