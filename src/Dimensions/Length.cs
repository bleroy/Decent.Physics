using static Decent.Physics.Unit;

namespace Decent.Physics.Dimensions
{
    public class Length : Dimension
    {
        public Length(Quantity quantity) : base(quantity) { }
        public Length(double meters)
        {
            Quantity = new Quantity(meters, m);
        }

        public override Unit CanonicalUnit
        {
            get { return m; }
        }

        public static explicit operator Length(Quantity length)
        {
            return new Length(length);
        }

        public static explicit operator double(Length d)
        {
            return d.Value;
        }
    }
}
