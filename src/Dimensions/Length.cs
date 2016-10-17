using static Decent.Physics.Unit;

namespace Decent.Physics.Dimensions
{
    public class Length : Dimension
    {
        public Length(Quantity quantity) : base(quantity) { }
        public Length(double meters) : base(meters, m) { }

        public override Unit CanonicalUnit
        {
            get { return m; }
        }
    }
}
