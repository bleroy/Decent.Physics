namespace Decent.Physics.Units
{
    public class PhysicalValue
    {
        public PhysicalValue(decimal value, Unit unit) { }
        public PhysicalValue(decimal value, Unit unit, decimal plusOrMinus) { }
        public PhysicalValue(decimal value, Unit unit, decimal lowerBound, decimal higherBound) { }

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
    }
}
