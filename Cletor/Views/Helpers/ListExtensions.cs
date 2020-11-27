using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cletor.Views.Helpers
{
    public static class ListExtensions
    {
        public static bool IsEquivalentTo<T>(this List<T> list1, List<T> list2)
            where T : IEquatable<T>
        {
            if (list1.Except(list2).Any())
                return false;

            if (list2.Except(list1).Any())
                return false;

            return true;
        }

        public static bool IsEquivalentTo<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
            where T : IEquatable<T>
        {
            if (list1.Except(list2).Any())
                return false;

            if (list2.Except(list1).Any())
                return false;

            return true;
        }

        public static bool IsEquivalentTo<T>(this ObservableCollection<T> list1, ObservableCollection<T> list2)
        {
            if (list1.Except(list2).Any())
                return false;

            if (list2.Except(list1).Any())
                return false;

            return true;
        }
    }
}
