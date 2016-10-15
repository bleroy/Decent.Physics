using System;
using System.Collections.Generic;
using System.Linq;

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

        public Unit(double factor, Unit baseUnit)
        {
            _factor = factor;
            _units = new List<BaseUnit>(baseUnit._units.Count);
            for (var i = 0; i < baseUnit._units.Count; i++)
            {
                _units[i] = baseUnit._units[i].Clone();
            }
        }

        private List<BaseUnit> _units;
        private double _factor = 1;

        public static Unit operator *(Unit first, Unit second)
        {
            var result = new Unit();
            result._factor = first._factor * second._factor;
            // Scan the base units in the first unit.
            foreach(var baseUnit in first._units)
            {
                // Is it common to both units?
                var otherBaseUnit = second.FindBaseUnit(baseUnit.Symbol);
                if (otherBaseUnit == null)
                {
                    // It's not, just copy it over.
                    result._units.Add(baseUnit.Clone());
                }
                else
                {
                    // It is common. Combine them.
                    // Combined power is the sum of powers.
                    var power = baseUnit.Power + otherBaseUnit.Power;
                    // Compare prefixes.
                    if (baseUnit.Prefix != otherBaseUnit.Prefix)
                    {
                        // Prefixes are different, so multiply them and integrate that
                        // into the factor for the new unit.
                        result._factor *= Math.Pow(10,
                            Prefix.Prefixes[baseUnit.Prefix].FactorPowerOfTen
                            + Prefix.Prefixes[otherBaseUnit.Prefix].FactorPowerOfTen);
                        // If prefix powers cancel each other, just skip.
                        if (power == 0) continue;
                        // Add the new base unit to the result.
                        result._units.Add(new BaseUnit(baseUnit.Symbol, power));
                    }
                    else
                    {
                        // Prefixes are the same.
                        // If prefix powers cancel each other, just skip.
                        if (power == 0) continue;
                        // Otherwise, we can just copy the prefix over.
                        result._units.Add(new BaseUnit(baseUnit.Prefix, baseUnit.Symbol, power));
                    }
                }
            }
            // Scan the base units in the second unit to find any we've missed.
            foreach(var baseUnit in second._units)
            {
                if (first.FindBaseUnit(baseUnit.Symbol) == null)
                {
                    // We found one that we haven't already taken care of. We can just copy it over.
                    result._units.Add(baseUnit.Clone());
                }
            }
            return result;
        }

        public static Quantity operator *(decimal value, Unit unit)
        {
            return new Quantity((double)value, unit);
        }
        public static Quantity operator *(double value, Unit unit)
        {
            return new Quantity(value, unit);
        }
        public static Quantity operator *(int value, Unit unit)
        {
            return new Quantity(value, unit);
        }

        public static Unit operator /(Unit first, Unit second)
        {
            var inverseSecond = second.Invert();
            return first * inverseSecond;
        }

        private Unit Invert()
        {
            // Dividing two units is the same as multiplying the first by the inverse of the second.
            var inverseSecond = new Unit();
            inverseSecond._factor = 1 / _factor;
            inverseSecond._units = new List<BaseUnit>(_units.Count);
            for (var i = 0; i < _units.Count; i++)
            {
                inverseSecond._units[i] = new BaseUnit(
                    _units[i].Prefix,
                    _units[i].Symbol,
                    -_units[i].Power);
            }

            return inverseSecond;
        }

        public static Unit operator /(double value, Unit unit)
        {
            var inverseSecond = unit.Invert();
            inverseSecond._factor *= value;
            return inverseSecond;
        }

        public static Unit operator ^(Unit unit, int power)
        {
            var result = new Unit();
            result._factor = unit._factor;
            result._units = new List<BaseUnit>(unit._units.Count);
            for (var i = 0; i < unit._units.Count; i++)
            {
                result._units[i] = new BaseUnit(
                    unit._units[i].Prefix,
                    unit._units[i].Symbol,
                    unit._units[i].Power * power);
            }

            return result;
        }

        public static implicit operator Quantity(Unit unit)
        {
            return new Quantity(1, unit);
        }

        private BaseUnit FindBaseUnit(string symbol)
        {
            foreach(var baseUnit in _units)
            {
                if (baseUnit.Symbol == symbol) return baseUnit;
            }
            return null;
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
            if (_units.Count != unit._units.Count) return false;
            if (_factor != unit._factor) return false;
            for(var i = 0; i < _units.Count; i++)
            {
                if (!_units[i].Equals(unit._units[i])) return false;
            }
            return true;
        }

        public bool IsSameDimensionAs(Unit unit)
        {
            if (_units.Count != unit._units.Count) return false;
            for (var i = 0; i < _units.Count; i++)
            {
                if (_units[i].Symbol != unit._units[i].Symbol || _units[i].Power != unit._units[i].Power) return false;
            }
            return true;
        }

        private static int CombineHashCodes(IEnumerable<int> hashCodes)
        {
            int hash = 5381;

            foreach (var hashCode in hashCodes)
            {
                hash = ((hash << 5) + hash) ^ hashCode;
            }
            return hash;
        }

        public override int GetHashCode()
        {
            return CombineHashCodes(_units
                .Select(baseUnit => baseUnit.GetHashCode())
                .Append(_factor.GetHashCode()));
        }

        public static Unit Parse(string unit)
        {
        }

        public override string ToString()
        {
            return string.Join("·", _units.Select(baseUnit => baseUnit.ToString()));
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

            public bool Equals(BaseUnit baseUnit)
            {
                return (Prefix == baseUnit.Prefix && Symbol == baseUnit.Symbol && Power == baseUnit.Power);
            }

            public BaseUnit Clone()
            {
                return new BaseUnit(Prefix, Symbol, Power);
            }

            public override string ToString()
            {
                return Prefix + Symbol + Helpers.Power.Format(Power);
            }
        }

        // Fundamental units
        public static readonly Unit None = new Unit();
        public static readonly Unit m = new Unit("m");
        public static readonly Unit kg = new Unit("kg");
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
        public static readonly Unit g = new Unit("m", "kg");
        public static readonly Unit km = new Unit("k", "m");
        public static readonly Unit cm = new Unit("c", "m");
        public static readonly Unit dm = new Unit("d", "m");
        public static readonly Unit mm = new Unit("m", "m");
        public static readonly Unit tonne = new Unit("k", "kg");
        public static readonly Unit l = dm ^ 3;
        public static readonly Unit a = new Unit("da", "m") ^ 2;
        public static readonly Unit ha = new Unit("h", "m") ^ 2;

        // Relative units that are multiples of fundamental or derived units:
        public static readonly Unit minute = new Unit(60, s);
        public static readonly Unit hour = new Unit(60, minute);
        public static readonly Unit day = new Unit(24, hour);
        public static readonly Unit week = new Unit(7, day);
        public static readonly Unit au = new Unit(1.496E11, m);
        public static readonly Unit pc = new Unit(3.0857E16, m);
        public static readonly Unit ly = new Unit(9.4607E15, m);
        public static readonly Unit eV = new Unit(1.6021766208E-19, J);
        public static readonly Unit u = new Unit(1.660539040E-27, kg);
        public static readonly Unit bar = new Unit(100000, Pa);
        public static readonly Unit Å = new Unit(1E-10, m);

        // Global registry of units
        public static readonly IDictionary<string, Unit> Units = new Dictionary<string, Unit>
        {
            {"", None },
            {"m", m },
            {"kg", kg },
            {"K", K },
            {"s", s },
            {"A", A },
            {"mol", mol },
            {"cd", cd },
            {"Hz", Hz },
            {"N", N },
            {"Pa", Pa },
            {"J", J },
            {"W", W },
            {"C", C },
            {"V", V },
            {"F", F },
            {"Ω", Ω },
            {"S", S },
            {"Wb", Wb },
            {"T", T },
            {"H", H },
            {"lx", lx },
            {"Bq", Bq },
            {"Gy", Gy },
            {"Sv", Sv },
            {"katal", katal },
            {"g", g },
            {"km", km },
            {"cm", cm },
            {"dm", dm },
            {"mm", mm },
            {"tonne", tonne },
            {"l", l },
            {"a", a },
            {"ha", ha },
            {"au", au },
            {"pc", pc },
            {"ly", ly },
            {"eV", eV },
            {"u", u },
            {"bar", bar },
            {"Å", Å}
        };
    }
}
