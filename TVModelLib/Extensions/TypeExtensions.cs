using System;

namespace TVModelLib.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetTvType(string name) 
            => Type.GetType($"TVModelLib.Model.TeilnehmerNS.Personen.{name}");
        public static Type GetAssemType(string name)
            => Type.GetType(name);
    }
}
