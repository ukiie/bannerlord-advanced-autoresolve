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
    }
}
