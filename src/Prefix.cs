using System.Collections.Generic;
using System.Diagnostics;

namespace Decent.Physics
{
    public class Prefix
    {
        private Prefix(string symbol, int factorPowerOfTen)
        {
            Symbol = symbol;
            FactorPowerOfTen = factorPowerOfTen;
        }

        public string Symbol { get; private set; }
        public int FactorPowerOfTen { get; private set; }

        public double Factor
        {
            get {
                Debug.Assert(-24 <= FactorPowerOfTen && FactorPowerOfTen <= 24);
                return new double[]
                {
                    1E-24, 1E-23, 1E-22, 1E-21, 1E-20, 1E-19, 1E-18, 1E-17, 1E-16, 1E-15, 1E-14,
                    1E-13, 1E-12, 1E-11, 1E-10, 1E-9, 1E-8, 1E-7, 1E-6, 1E-5, 1E-4, 1E-3, 1E-2, 1E-1,
                    1, 10, 100, 1000, 1E4, 1E5, 1E6, 1E7, 1E8, 1E9, 1E10, 1E11, 1E12, 1E13, 1E14,
                    1E15, 1E16, 1E17, 1E18, 1E19, 1E20, 1E21, 1E22, 1E23, 1E24
                }[FactorPowerOfTen + 24];
            }
        }

        public override string ToString()
        {
            return Symbol;
        }

        public static readonly Prefix None = new Prefix("", 0);
        public static readonly Prefix da = new Prefix("da", 1);
        public static readonly Prefix h = new Prefix("h", 2);
        public static readonly Prefix k = new Prefix("k", 3);
        public static readonly Prefix M = new Prefix("M", 6);
        public static readonly Prefix G = new Prefix("G", 9);
        public static readonly Prefix T = new Prefix("T", 12);
        public static readonly Prefix P = new Prefix("P", 15);
        public static readonly Prefix E = new Prefix("E", 18);
        public static readonly Prefix Z = new Prefix("Z", 21);
        public static readonly Prefix Y = new Prefix("Y", 24);
        public static readonly Prefix d = new Prefix("d", -1);
        public static readonly Prefix c = new Prefix("c", -2);
        public static readonly Prefix m = new Prefix("m", -3);
        public static readonly Prefix μ = new Prefix("μ", -6);
        public static readonly Prefix n = new Prefix("n", -9);
        public static readonly Prefix p = new Prefix("p", -12);
        public static readonly Prefix f = new Prefix("f", -15);
        public static readonly Prefix a = new Prefix("a", -18);
        public static readonly Prefix z = new Prefix("z", -21);
        public static readonly Prefix y = new Prefix("y", -24);

        public static readonly IDictionary<string, Prefix> Prefixes = new Dictionary<string, Prefix>
        {
            {"", None },
            {"da", da },
            {"h", h },
            {"k", k },
            {"M", M },
            {"G", G },
            {"T", T },
            {"P", P },
            {"E", E },
            {"Z", Z },
            {"Y", Y },
            {"d", d },
            {"c", c },
            {"m", m },
            {"μ", μ },
            {"n", n },
            {"p", p },
            {"f", f },
            {"a", a },
            {"z", z },
            {"y", y }
        };
    }
}
