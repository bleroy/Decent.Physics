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
        }

        public double Value { get; private set; }
        public Unit Unit { get; private set; }
        public double LowerBound { get; private set; }
        public double HigherBound { get; private set; }

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

        public bool IsSameDimensionAs(Quantity quantity)
        {
            return Unit.IsSameDimensionAs(quantity.Unit);
        }

        public bool IsSameDimensionAs(Unit unit)
        {
            return Unit.IsSameDimensionAs(unit);
        }

        public static Quantity Parse(string val)
        {

        }

        public override string ToString()
        {
        }
    }
}
