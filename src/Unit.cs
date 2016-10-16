using System;
using System.Collections.Generic;
using System.Globalization;
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

        public Unit(string symbol, int power = 1)
        {
            _units = new List<BaseUnit>
            {
                new BaseUnit(symbol, power)
            };
        }

        public Unit(string prefix, string symbol, int power = 1)
        {
            _units = new List<BaseUnit>
            {
                new BaseUnit(prefix, symbol, power)
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

        internal Unit Invert()
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

        internal IList<BaseUnit> SortBaseUnits()
        {
            var result = new List<BaseUnit>(_units.Count);
            for(var i = 0; i < _units.Count; i++)
            {
                result[i] = _units[i].Clone();
            }
            result.Sort((baseUnit1, baseUnit2)
                => string.Compare(baseUnit1.Symbol, baseUnit2.Symbol, StringComparison.OrdinalIgnoreCase));
            return result;
        }

        public bool Equals(Unit unit)
        {
            if (_units.Count != unit._units.Count) return false;
            if (_factor != unit._factor) return false;
            var sorted1 = SortBaseUnits();
            var sorted2 = unit.SortBaseUnits();
            for(var i = 0; i < _units.Count; i++)
            {
                if (!sorted1[i].Equals(sorted2[i])) return false;
            }
            return true;
        }

        internal Unit ExpandToFundamentalUnits()
        {
            if (_units.Count == 1)
            {
                return this;
            }
            var accumulator = new Unit();
            accumulator._factor = _factor;
            foreach(var baseUnit in _units)
            {
                var unit = Units[baseUnit.Symbol].ExpandToFundamentalUnits();
                unit._factor = Prefix.Prefixes[baseUnit.Prefix].Factor;
                accumulator *= unit;
            }
            return accumulator;
        }

        public bool IsSameDimensionAs(Unit unit)
        {
            // This needs to work on fully expanded fundamental units.
            var thisExpanded = ExpandToFundamentalUnits();
            var thatExpanded = unit.ExpandToFundamentalUnits();
            if (thisExpanded._units.Count != thatExpanded._units.Count) return false;
            var sorted1 = thisExpanded.SortBaseUnits();
            var sorted2 = thatExpanded.SortBaseUnits();
            return IsSameExpandedAndSortedDimensionsAs(sorted1, sorted2);
        }

        internal static bool IsSameExpandedAndSortedDimensionsAs(IList<BaseUnit> sorted1, IList<BaseUnit> sorted2)
        {
            if (sorted1.Count != sorted2.Count) return false;
            for (var i = 0; i < sorted1.Count; i++)
            {
                if (sorted1[i].Symbol != sorted2[i].Symbol
                    || sorted1[i].Power != sorted2[i].Power) return false;
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
            return CombineHashCodes(SortBaseUnits()
                .Select(baseUnit => baseUnit.GetHashCode())
                .Append(_factor.GetHashCode()));
        }

        public static Unit Parse(string unit)
        {
            return Parse(unit, CultureInfo.CurrentCulture);
        }

        public static Unit Parse(string unit, IFormatProvider formatProvider)
        {
            Unit result;
            ParseInternal(unit, formatProvider, out result);
            return result;
        }

        public static bool TryParse(string unit, out Unit result)
        {
            return TryParse(unit, CultureInfo.CurrentCulture, out result);
        }

        public static bool TryParse(string unit, IFormatProvider formatProvider, out Unit result)
        {
            return ParseInternal(unit, formatProvider, out result, false);
        }

        internal static bool ParseInternal(string unit, IFormatProvider formatProvider, out Unit result, bool throwOnError = true)
        {
            var tempResult = new Unit();
            var start = 0;
            string symbol = null;
            var parsingSymbol = true;
            for (var i = 0; i < unit.Length; i++)
            {
                var c = unit[i];
                if (parsingSymbol && c == '^')
                {
                    symbol = unit.Substring(start, i - start);
                    start = i + 1;
                    parsingSymbol = false;
                }
                else if (c == '*' || c == '/' || i == unit.Length - 1)
                {
                    // End of base unit section
                    // Symbol should exist, otherwise that's an error.
                    if (string.IsNullOrEmpty(symbol))
                    {
                        if (throwOnError)
                        {
                            throw new FormatException($"Missing unit symbol in '{unit}' at index {i}.");
                        }
                        else
                        {
                            result = null;
                            return false;
                        }
                    }
                    int power;
                    if (!parsingSymbol)
                    {
                        var powerString = unit.Substring(start, i - start + (i == unit.Length - 1 ? 1 : 0));
                        // We were parsing a power
                        if (!int.TryParse(powerString, NumberStyles.Integer, formatProvider, out power))
                        {
                            if (throwOnError)
                            {
                                throw new FormatException($"Can't parse power '{powerString}' in '{unit}'.");
                            }
                            else
                            {
                                result = null;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        power = 1;
                    }
                    // Find what unit this is
                    if (Units.ContainsKey(symbol))
                    {
                        // That's a known unit symbol, no need to look for a prefix.
                        tempResult._units.Add(new BaseUnit(symbol, power));
                    }
                    else
                    {
                        // We need to look for a prefix.
                        var inError = false;
                        // All prefixes are just one character, except for "da".
                        if (symbol[0] == 'd')
                        {
                            symbol = symbol.Substring(1);
                            if (Units.ContainsKey(symbol))
                            {
                                tempResult._units.Add(new BaseUnit("d", symbol, power));
                            }
                            else
                            {
                                if (symbol[0] == 'a')
                                {
                                    // This is da*.
                                    symbol = symbol.Substring(1);
                                    if (Units.ContainsKey(symbol))
                                    {
                                        tempResult._units.Add(new BaseUnit("da", symbol, power));
                                    }
                                    else
                                    {
                                        inError = true;
                                    }
                                }
                                else
                                {
                                    inError = true;
                                }
                            }
                        }
                        else
                        {
                            var prefix = symbol[0].ToString();
                            if (Prefix.Prefixes.ContainsKey(prefix))
                            {
                                symbol = symbol.Substring(1);
                                if (Units.ContainsKey(symbol))
                                {
                                    tempResult._units.Add(new BaseUnit(prefix, symbol, power));
                                }
                                else
                                {
                                    inError = true;
                                }
                            }
                        }
                        if (inError)
                        {
                            if (throwOnError)
                            {
                                throw new FormatException($"Can't parse unit '{symbol}' in '{unit}'.");
                            }
                            else
                            {
                                result = null;
                                return false;
                            }
                        }
                    }
                    parsingSymbol = true;
                    symbol = null;
                }
            }
            result = tempResult;
            return true;
        }

        public override string ToString()
        {
            return string.Join("·", _units.Select(baseUnit => baseUnit.ToString()));
        }

        internal class BaseUnit
        {
            public BaseUnit(string prefix, string symbol, int power = 1)
            {
                Prefix = prefix;
                Symbol = symbol;
                Power = power;
            }

            public BaseUnit(string symbol, int power = 1) : this("", symbol, power) { }

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
