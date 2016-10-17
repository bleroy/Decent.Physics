namespace Decent.Physics.Dimensions
{
    public abstract class Dimension : Quantity
    {
        protected Dimension(Quantity quantity)
            : base(quantity.Value, quantity.Unit, quantity.LowerBound, quantity.HigherBound)
        {
            if (!quantity.Unit.Equals(CanonicalUnit))
            {
                throw new IncompatibleUnitsException(quantity.Unit, CanonicalUnit);
            }
        }

        protected Dimension(double value, Unit unit) : base(value, unit) { }
        protected Dimension(double value, Unit unit, double plusOrMinus)
            : base(value, unit, plusOrMinus) { }
        protected Dimension(double value, Unit unit, double lowerBound, double higherBound)
            : base(value, unit, lowerBound, higherBound) { }

        public abstract Unit CanonicalUnit { get; }

        public static explicit operator double(Dimension d)
        {
            return d.ConvertTo(d.CanonicalUnit).Value;
        }
    }
}
