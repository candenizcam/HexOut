using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Collections.Generic;
using Unity.Mathematics;

namespace Punity
{
    public static class EnumerationTools
    {
        public static IEnumerable<(int i, T arg2)> NumberedList<T>(this List<T> enumerable)
        {
            var c = enumerable.Count;
            var r = Enumerable.Range(0, c);
            return r.Zip(enumerable, (i, arg2) => (i, arg2));
        }
        
        public static IEnumerable<T> Shuffled<T>(this List<T> enumerable)
        {
            var rng = new System.Random();
            return enumerable.OrderBy(a => rng.Next());
        }

        public static T PickRandom<T>(this List<T> enumerable)
        {
            var rng = new System.Random();
            var index = rng.Next(0, enumerable.Count());
            return enumerable[index];
        }

        public static T PickRandom<T>(this T[] enumerable)
        {
            var rng = new System.Random();
            var index = rng.Next(0, enumerable.Length);
            return enumerable[index];
        }
        
    }
}