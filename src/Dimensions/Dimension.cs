namespace Decent.Physics.Dimensions
{
    public abstract class Dimension
    {
        protected Dimension() { }
        protected Dimension(Quantity quantity)
        {
            if (!quantity.Unit.Equals(CanonicalUnit))
            {
                throw new IncompatibleUnitsException(quantity.Unit, CanonicalUnit);
            }
            Quantity = quantity;
        }

        public abstract Unit CanonicalUnit { get; }

        protected Quantity Quantity { get; set; }

        public double Value
        {
            get
            {
                return ((Quantity)this).Value;
            }
        }

        public static explicit operator Quantity(Dimension d)
        {
            return d.Quantity;
        }

        public static explicit operator double(Dimension d)
        {
            return d.Value;
        }
    }
}
