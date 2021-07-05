using System.Collections.Generic;

namespace TVModelLib.Extensions
{
    public static class GenericListExtensions
    {
        public static void MoveTo<T>(this List<T> value, T item, List<T> targetList)
        {
            value.Remove(item);
            targetList.Add(item);
        }
    }
}
