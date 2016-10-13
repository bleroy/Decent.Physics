using static Decent.Physics.Unit;

namespace Decent.Physics
{
    public class PhysicalValue
    {
        public PhysicalValue(double value, Unit unit) { }
        public PhysicalValue(double value, Unit unit, decimal plusOrMinus) { }
        public PhysicalValue(double value, Unit unit, decimal lowerBound, decimal higherBound) { }

        public static PhysicalValue operator *(PhysicalValue val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(decimal val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(float val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(decimal val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(double val1, PhysicalValue val2) { }
        public static PhysicalValue operator *(int val1, PhysicalValue val2) { }
        public static PhysicalValue operator /(PhysicalValue val1, PhysicalValue val2) { }
        public static PhysicalValue operator /(decimal val1, PhysicalValue val2) { }
        public static PhysicalValue operator /(PhysicalValue val1, decimal val2) { }
        public static PhysicalValue operator ^(PhysicalValue val1, int val2) { }
        public static PhysicalValue operator +(PhysicalValue val1, PhysicalValue val2) { }
        public static PhysicalValue operator -(PhysicalValue val1, PhysicalValue val2) { }

        public double ConvertTo(Unit unit)
        { }

        public double ConvertTo(PhysicalValue unit)
        {
        }

        public static PhysicalValue Parse(string val)
        {

        }

        public override string ToString()
        {
        }
    }
}
