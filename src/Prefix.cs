using System;
using System.Diagnostics;

namespace Decent.Physics.Units
{
    public class Prefix
    {
        private Prefix(string name, string symbol, int factorPowerOfTen)
        {
            Name = name;
            Symbol = symbol;
            FactorPowerOfTen = Math.Abs(factorPowerOfTen);
            FactorMultiplies = factorPowerOfTen < 0;
        }

        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public int FactorPowerOfTen { get; private set; }
        public bool FactorMultiplies { get; private set; }

        public double Factor
        {
            get {
                Debug.Assert(FactorPowerOfTen >= 0 && FactorPowerOfTen <= 24);
                return new double[] {1, 10, 100, 1000, 10^4, 10^5, 10^6, 10^7, 10^8, 10^9, 10^10,
                    10^11, 10^12, 10^13, 10^14, 10^15, 10^16, 10^17, 10^18, 10^19, 10^20,
                    10^21, 10^22, 10^23, 10^24 }
                    [FactorPowerOfTen];
            }
        }

        public static double GetValue(Prefix prefix, double val)
        {
            return prefix.FactorPowerOfTen == 0 ? val :
                prefix.FactorMultiplies ? val * prefix.Factor :
                val / prefix.Factor;
        }

        public double GetValue(double val)
        {
            return GetValue(this, val);
        }

        public override string ToString()
        {
            return Symbol;
        }

        public static readonly Prefix None = new Prefix("", "", 0);
        public static readonly Prefix da = new Prefix("deca", "da", 1);
        public static readonly Prefix h = new Prefix("hecto", "h", 2);
        public static readonly Prefix k = new Prefix("kilo", "k", 3);
        public static readonly Prefix M = new Prefix("mega", "M", 6);
        public static readonly Prefix G = new Prefix("giga", "G", 9);
        public static readonly Prefix T = new Prefix("tera", "T", 12);
        public static readonly Prefix P = new Prefix("peta", "P", 15);
        public static readonly Prefix E = new Prefix("exa", "E", 18);
        public static readonly Prefix Z = new Prefix("zetta", "Z", 21);
        public static readonly Prefix Y = new Prefix("yotta", "Y", 24);
        public static readonly Prefix d = new Prefix("deci", "d", -1);
        public static readonly Prefix c = new Prefix("centi", "c", -2);
        public static readonly Prefix m = new Prefix("milli", "m", -3);
        public static readonly Prefix μ = new Prefix("micro", "μ", -6);
        public static readonly Prefix n = new Prefix("nano", "n", -9);
        public static readonly Prefix p = new Prefix("pico", "p", -12);
        public static readonly Prefix f = new Prefix("femto", "f", -15);
        public static readonly Prefix a = new Prefix("atto", "a", -18);
        public static readonly Prefix z = new Prefix("zepto", "z", -21);
        public static readonly Prefix y = new Prefix("yocto", "y", -24);
    }
}
