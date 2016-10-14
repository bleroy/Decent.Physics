using System;

namespace Decent.Physics
{
    public class IncompatibleUnitsException : Exception
    {
        public IncompatibleUnitsException() : this(null, Unit.None, Unit.None) { }
        public IncompatibleUnitsException(string message) : this(message, Unit.None, Unit.None) { }
        public IncompatibleUnitsException(Unit unit, Unit targetUnit) : this(null, unit, targetUnit) { }
        public IncompatibleUnitsException(string message, Unit unit, Unit targetUnit)
        {
            _message = message ?? "Incompatible units.";
            Unit = unit;
        }

        public Unit Unit { get; private set; }
        public Unit TargetUnit { get; private set; }

        private string _message;
        public override string Message { get { return _message; } }

        public override string ToString()
        {
            return Message;
        }
    }
}
