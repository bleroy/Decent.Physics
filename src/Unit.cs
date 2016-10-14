using System;
using System.Collections.Generic;

namespace Decent.Physics
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

        public static Quantity operator *(decimal value, Unit unit)
        {
            return new Quantity((double)value, unit);
        }
        public static Quantity operator *(double val1, Unit val2)
        {
            return new Quantity(val1, val2);
        }
        public static Quantity operator *(int val1, Unit val2)
        {
            return new Quantity(val1, val2);
        }

        public static Unit operator /(Unit first, Unit second)
        {
        }
        public static Unit operator /(double first, Unit second)
        {
        }

        public static Unit operator ^(Unit first, int second)
        {

        }

        public static implicit operator Quantity(Unit unit)
        {
            return new Quantity(1, unit);
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

        // Fundamental units
        public static readonly Unit None = new Unit();
        public static readonly Unit m = new Unit("m");
        public static readonly Unit g = new Unit("g");
        public static readonly Unit K = new Unit("K");
        public static readonly Unit s = new Unit("s");
        public static readonly Unit A = new Unit("A");
        public static readonly Unit mol = new Unit("mol");
        public static readonly Unit cd = new Unit("cd");

        // Derived units
        public static readonly Unit Hz = s ^ -1;
        public static readonly Unit N = kg * m / (s ^ 2);
        public static readonly Unit Pa = N / (m ^ 2);
        public static readonly Unit J = N * m;
        public static readonly Unit W = J / s;
        public static readonly Unit C = s * A;
        public static readonly Unit V = W / A;
        public static readonly Unit F = C / V;
        public static readonly Unit Ω = V / A;
        public static readonly Unit S = A / V;
        public static readonly Unit Wb = V * s;
        public static readonly Unit T = Wb / (m ^ 2);
        public static readonly Unit H = Wb / A;
        public static readonly Unit lx = cd / (m ^ 2);
        public static readonly Unit Bq = Hz;
        public static readonly Unit Gy = J / kg;
        public static readonly Unit Sv = Gy;
        public static readonly Unit katal = mol / s;

        // Some common prefixed derived units
        public static readonly Unit kg = new Unit("k", "g");
        public static readonly Unit km = new Unit("k", "m");
        public static readonly Unit cm = new Unit("c", "m");
        public static readonly Unit dm = new Unit("d", "m");
        public static readonly Unit mm = new Unit("m", "m");
        public static readonly Unit tonne = new Unit("M", "g");
        public static readonly Unit l = dm ^ 3;
        public static readonly Unit a = new Unit("da", "m") ^ 2;
        public static readonly Unit ha = new Unit("h", "m") ^ 2;

        // Not really constants, not units because there's a value:
        public static readonly Quantity minute = 60 * s;
        public static readonly Quantity hour = 60 * minute;
        public static readonly Quantity day = 24 * hour;
        public static readonly Quantity week = 7 * day;
        public static readonly Quantity au = 1.496E11 * m;
        public static readonly Quantity pc = 3.0857E16 * m;
        public static readonly Quantity ly = 9.4607E15 * m;
        public static readonly Quantity eV = 1.6021766208E-19 * J;
        public static readonly Quantity u = 1.660539040E-27 * kg;
        public static readonly Quantity bar = 100000 * Pa;
        public static readonly Quantity Å = 1E-10 * m;
    }
}
