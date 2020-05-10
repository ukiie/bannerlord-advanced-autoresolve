using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Helpers
{
    internal static class EnumExtensions
    {
        internal static int GetEnumCount<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        internal static List<T> GetAll<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        internal static List<string> GetAllAsListOfString<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }
    }
}
