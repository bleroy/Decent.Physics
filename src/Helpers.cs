using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Decent.Physics
{
    internal static class Helpers
    {
        public static string FormatPower(int power)
        {
            return power == 1 ? "" :
                new string(power.ToString("D").ToCharArray().Select(Exponent).ToArray());
        }

        private static char Exponent(char c)
        {
            if (c == '-') return '⁻';
            Debug.Assert(c >= '0' && c <= '9');
            return new char[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹' }[c - '0'];
        }

        public static int CombineHashCodes(IEnumerable<int> hashCodes)
        {
            int hash = 5381;

            foreach (var hashCode in hashCodes)
            {
                hash = ((hash << 5) + hash) ^ hashCode;
            }
            return hash;
        }

        public static int CombineHashCodes(params int[] hashCodes)
        {
            return CombineHashCodes(hashCodes);
        }

        public static int CombineHashCodes(params object[] objects)
        {
            return CombineHashCodes(objects.Select(o => o.GetHashCode()));
        }
    }
}
