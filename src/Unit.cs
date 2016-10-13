using System.Collections.Generic;

namespace Decent.Physics.Units
{
    /// <summary>
    /// Represents a unit, that can be a base unit such as m, kg, s, A, or a derived unit such as m·s⁻², kg·m⁻¹·s⁻², etc.
    /// </summary>
    public class Unit
    {
        public Unit()
        {
            _units = new List<BaseUnit>();
        }

        public Unit(string name, int power = 1)
        {
            _units = new List<BaseUnit>
            {
                new BaseUnit(name, power)
            };
        }

        public Unit(string prefix, string name, int power = 1)
        {
            _units = new List<BaseUnit>
            {
                new BaseUnit(prefix, name, power)
            };
        }

        private List<BaseUnit> _units;

        public static Unit operator *(Unit first, Unit second)
        {
        }

        public static Unit operator /(Unit first, Unit second)
        {
        }

        public static Unit operator ^(Unit first, int second)
        {

        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var unit = obj as Unit;
            if ((object)unit == null) return false;
            return Equals(unit);
        }

        public bool Equals(Unit unit)
        {

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Unit Parse(string unit) { }

        public override string ToString()
        {
            
        }

        private class BaseUnit
        {
            public BaseUnit(string prefix, string symbol, int power = 1)
            {
                Prefix = prefix;
                Symbol = symbol;
                Power = power;
            }

            public BaseUnit(string name, int power = 1) : this("", name, power) { }

            public string Prefix { get; private set; }
            public string Symbol { get; private set; }
            public int Power { get; private set; }

            public string ToFullName()
            {
            }

            public override string ToString()
            {
                return Prefix + Symbol + Helpers.Power.Format(Power);
            }
        }

        public static readonly Unit None = new Unit();
        public static readonly Unit m = new Unit("m");
        public static readonly Unit g = new Unit("g");
        public static readonly Unit kg = new Unit("k", "g");
        public static readonly Unit K = new Unit("K");
        public static readonly Unit s = new Unit("s");
        public static readonly Unit A = new Unit("A");
        public static readonly Unit mol = new Unit("mol");
        public static readonly Unit cd = new Unit("cd");

        public static readonly Unit Hz = s ^ -1;
        public static readonly Unit N = kg * m / s ^ 2;
        public static readonly Unit Pa = N / m ^ 2;
        public static readonly Unit J = N * m;
        public static readonly Unit W = J / s;
        public static readonly Unit C = s * A;
        public static readonly Unit V = W / A;
        public static readonly Unit F = C / V;
        public static readonly Unit Ω = V / A;
        public static readonly Unit S = A / V;
        public static readonly Unit Wb = V * s;
        public static readonly Unit T = Wb / m ^ 2;
        public static readonly Unit H = Wb / A;
        public static readonly Unit lx = cd / m ^ 2;
        public static readonly Unit Bq = Hz;
        public static readonly Unit Gy = J / kg;
        public static readonly Unit Sv = Gy;
        public static readonly Unit katal = mol / s;
    }
}
