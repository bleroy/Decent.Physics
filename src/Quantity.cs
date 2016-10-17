using System;
using System.Globalization;
using System.Linq;

namespace Decent.Physics
{
    public class Quantity
    {
        public Quantity(double value, Unit unit) : this(value, unit, value, value) { }
        public Quantity(double value, Unit unit, double plusOrMinus) : this(value, unit, value - plusOrMinus, value + plusOrMinus) { }
        public Quantity(double value, Unit unit, double lowerBound, double higherBound)
        {
            Value = value;
            Unit = unit;
            LowerBound = lowerBound;
            HigherBound = higherBound;
            FixBounds();
        }

        public double Value { get; private set; }
        public Unit Unit { get; private set; }
        public double LowerBound { get; private set; }
        public double HigherBound { get; private set; }

        public static Quantity operator *(Quantity val1, Quantity val2)
        {
            return new Quantity(
                val1.Value * val2.Value,
                val1.Unit * val2.Unit,
                val1.LowerBound * val2.LowerBound,
                val1.HigherBound * val2.HigherBound);
        }

        public static Quantity operator *(double val1, Quantity val2)
        {
            return new Quantity(
                val1 * val2.Value,
                val2.Unit,
                val1 * val2.LowerBound,
                val1 * val2.HigherBound);
        }
        public static Quantity operator *(float val1, Quantity val2)
        {
            return (double)val1 * val2;
        }
        public static Quantity operator *(decimal val1, Quantity val2)
        {
            return (double)val1 * val2;
        }
        public static Quantity operator *(int val1, Quantity val2)
        {
            return (double)val1 * val2;
        }

        public static Quantity operator /(Quantity val1, Quantity val2)
        {
            return new Quantity(
                val1.Value / val2.Value,
                val1.Unit / val2.Unit,
                val1.LowerBound / val2.HigherBound,
                val1.HigherBound / val2.LowerBound);
        }
        public static Quantity operator /(double val1, Quantity val2)
        {
            return new Quantity(
                val1 / val2.Value,
                val2.Unit.Invert(),
                val1 / val2.HigherBound,
                val1 / val2.LowerBound);
        }
        public static Quantity operator /(Quantity val1, double val2)
        {
            return (1 / val2) * val1;
        }

        public static Quantity operator ^(Quantity val1, int val2)
        {
            return new Quantity(
                Math.Pow(val1.Value, val2),
                val1.Unit^val2,
                Math.Pow(val1.LowerBound, val2),
                Math.Pow(val1.HigherBound, val2));
        }

        public static Quantity operator +(Quantity val1, Quantity val2)
        {
            if (!val1.Unit.IsSameDimensionAs(val2.Unit))
            {
                throw new InvalidOperationException(
                    $"Can't add quantities with incompatible units {val1.Unit} and {val2.Unit}.");
            }
            // Normalize the unit to the second operand
            var converted = val1.ConvertTo(val2);
            return new Quantity(
                converted.Value + val2.Value,
                val2.Unit,
                converted.LowerBound + val2.LowerBound,
                converted.HigherBound + val2.HigherBound);
        }
        public static Quantity operator -(Quantity val1, Quantity val2)
        {
            return val1 + (-val2);
        }
        public static Quantity operator -(Quantity value)
        {
            return new Quantity(
                -value.Value,
                value.Unit,
                -value.HigherBound,
                -value.LowerBound);
        }

        public static bool operator ==(Quantity val1, Quantity val2)
        {
            if (ReferenceEquals(val1, val2)) return true;
            if ((object)val1 == null) return false;
            return val1.Equals(val2);
        }

        public static bool operator !=(Quantity a, Quantity b)
        {
            return !(a == b);
        }

        private void FixBounds()
        {
            if (LowerBound > HigherBound)
            {
                var t = LowerBound;
                LowerBound = HigherBound;
                HigherBound = t;
            }
        }

        public Quantity ConvertTo(Unit unit)
        {
            var thisExpanded = Unit.ExpandToFundamentalUnits().SortBaseUnits();
            var thatExpanded = unit.ExpandToFundamentalUnits().SortBaseUnits();
            if (!Unit.IsSameExpandedAndSortedDimensionsAs(thisExpanded, thatExpanded))
            {
                throw new InvalidOperationException(
                    $"Can't convert {Unit} to {unit}.");
            }
            var factor = thisExpanded.Aggregate(1d,
                (val, u) => val * Math.Pow(Prefix.Prefixes[u.Prefix].Factor, u.Power))
                / thatExpanded.Aggregate(1d,
                (val, u) => val * Math.Pow(Prefix.Prefixes[u.Prefix].Factor, u.Power));
            return new Quantity(
                Value * factor,
                unit,
                LowerBound * factor,
                HigherBound * factor);
        }

        public Quantity ConvertTo(Quantity unit)
        {
            return ConvertTo(unit.Unit) / unit.Value;
        }

        public bool IsSameDimensionAs(Quantity quantity)
        {
            return Unit.IsSameDimensionAs(quantity.Unit);
        }

        public bool IsSameDimensionAs(Unit unit)
        {
            return Unit.IsSameDimensionAs(unit);
        }

        public static Quantity Parse(string val, IFormatProvider formatProvider)
        {
            Quantity result;
            ParseInternal(val, formatProvider, out result);
            return result;
        }

        public static Quantity Parse(string val)
        {
            return Parse(val, CultureInfo.CurrentCulture);
        }

        public static bool TryParse(string val, out Quantity quantity)
        {
            return TryParse(val, CultureInfo.CurrentCulture, out quantity);
        }

        public static bool TryParse(string val, IFormatProvider formatProvider, out Quantity quantity)
        {
            return ParseInternal(val, formatProvider, out quantity, false);
        }

        private static bool ParseInternal(string val, IFormatProvider formatProvider, out Quantity quantity, bool throwOnError = true)
        {
            var foundAnE = false;
            int i;
            for (i = 0; i < val.Length; i++)
            {
                if (char.IsLetter(val[i]))
                {
                    if (foundAnE) break;
                    if (val[i] == 'E' && i < val.Length - 1 && char.IsDigit(val[i + 1]))
                    {
                        foundAnE = true;
                    }
                }
            }
            double resultValue;
            if (throwOnError)
            {
                try
                {
                    resultValue = double.Parse(val.Substring(0, i));
                }
                catch(FormatException e)
                {
                    throw new FormatException($"Couldn't parse the numeric value from {val}.", e);
                }
                catch(OverflowException e)
                {
                    throw new FormatException($"The numeric value from {val} caused an overflow.", e);
                }
            }
            else
            {
                if (!double.TryParse(val.Substring(0, i), NumberStyles.Float, formatProvider, out resultValue))
                {
                    quantity = null;
                    return false;
                }
            }
            Unit resultUnit;
            if (!Unit.ParseInternal(val.Substring(i), formatProvider, out resultUnit, throwOnError))
            {
                quantity = null;
                return false;
            }
            quantity = new Quantity(resultValue, resultUnit);
            return true;
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return Value.ToString(formatProvider) + Unit.ToString();
        }

        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }
    }
}
