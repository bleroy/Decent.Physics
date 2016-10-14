namespace Decent.Physics
{
    public class Quantity
    {
        public Quantity(double value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }
        public Quantity(double value, Unit unit, decimal plusOrMinus) { }
        public Quantity(double value, Unit unit, decimal lowerBound, decimal higherBound) { }

        public double Value { get; private set; }
        public Unit Unit { get; private set; }

        public static Quantity operator *(Quantity val1, Quantity val2) { }
        public static Quantity operator *(decimal val1, Quantity val2) { }
        public static Quantity operator *(float val1, Quantity val2) { }
        public static Quantity operator *(decimal val1, Quantity val2) { }
        public static Quantity operator *(double val1, Quantity val2) { }
        public static Quantity operator *(int val1, Quantity val2) { }
        public static Quantity operator /(Quantity val1, Quantity val2) { }
        public static Quantity operator /(decimal val1, Quantity val2) { }
        public static Quantity operator /(Quantity val1, decimal val2) { }
        public static Quantity operator ^(Quantity val1, int val2) { }
        public static Quantity operator +(Quantity val1, Quantity val2) { }
        public static Quantity operator -(Quantity val1, Quantity val2) { }

        public double ConvertTo(Unit unit)
        { }

        public double ConvertTo(Quantity unit)
        {
        }

        public static Quantity Parse(string val)
        {

        }

        public override string ToString()
        {
        }
    }
}
