using static Decent.Physics.Unit;

namespace Decent.Physics
{
    public class PhysicalValue
    {
        public PhysicalValue(decimal value, Unit unit) { }
        public PhysicalValue(decimal value, Unit unit, decimal plusOrMinus) { }
        public PhysicalValue(decimal value, Unit unit, decimal lowerBound, decimal higherBound) { }

        public static PhysicalValue operator *(PhysicalValue val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(decimal val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(PhysicalValue val1, decimal val2) { }
        public static PhysicalValue operator /(PhysicalValue val1, PhysicalValue val2) { }
        public static PhysicalValue operator /(decimal val1, PhysicalValue val2) { }
        public static PhysicalValue operator /(PhysicalValue val1, decimal val2) { }
        public static PhysicalValue operator ^(PhysicalValue val1, int val2) { }
        public static PhysicalValue operator +(PhysicalValue val1, PhysicalValue val2) { }
        public static PhysicalValue operator -(PhysicalValue val1, PhysicalValue val2) { }

        public decimal Convert(PhysicalValue val, Unit unit)
        { }

        public decimal Convert(PhysicalValue val, PhysicalValue unit)
        {
        }

        public static PhysicalValue Parse(string val)
        {

        }

        public override string ToString()
        {
        }

        public static readonly PhysicalValue minute = 60 * s;
        public static readonly PhysicalValue hour = 60 * minute;
        public static readonly PhysicalValue day = 24 * hour;
        public static readonly PhysicalValue week = 7 * day;
    }
}
