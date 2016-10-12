using System.Diagnostics;
using System.Linq;

namespace Decent.Physics.Helpers
{
    public static class Power
    {
        public static string Format(int power) {
            return power == 1 ? "" :
                new string(power.ToString("D").ToCharArray().Select(Exponent).ToArray());
        }

        private static char Exponent(char c)
        {
            if (c == '-') return '⁻';
            Debug.Assert(c >= '0' && c <= '9');
            return ['⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹'][c - '0'];
        }
    }
}
