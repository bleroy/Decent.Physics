using System;
using static Decent.Physics.Unit;

namespace Decent.Physics
{
    public static class Constants
    {
        public static double π = Math.PI;
        public static PhysicalValue c = 299792458 * m / s;
        public static PhysicalValue G = 6.67408E-11 * (m ^ 3) / kg / (s ^ 2);
        public static PhysicalValue g = 9.80665 * m / (s ^ 2);
        public static PhysicalValue h = 6.626070040E-34 * J * s;
        public static PhysicalValue ℏ = 1.054571800E-34 * J * s;
        public static PhysicalValue ε0 = 8.854187817E-12 * F / m;
        public static PhysicalValue μ0 = 4E-7 * π * N / (A ^ 2);
        public static PhysicalValue e = 1.6021766208E-19 * C;
        public static PhysicalValue me = 9.10938356E-31 * kg;
        public static PhysicalValue mp = 1.672621898E-27 * kg;
        public static PhysicalValue mn = 1.674927471E-27 * kg;
        public static PhysicalValue mu = 1.660539040E-27 * kg;
        public static PhysicalValue NA = 6.022140857E23 / mol;
        public static PhysicalValue k = 1.38064852E-23 * J / K;
        public static PhysicalValue R = 8.3144598 * J / mol / K;
        public static PhysicalValue σ = 5.670367E-8 * W / (m ^ 2) / (K ^ 4);
        public static PhysicalValue b = 2.8977729 * mm / K;
        public static PhysicalValue atm = 101325 * Pa;
        public static PhysicalValue H0 = 2.25E-18 / s;
        public static PhysicalValue α = (e ^ 2) / (4 * π * ε0 * ℏ * c);
    }
}
