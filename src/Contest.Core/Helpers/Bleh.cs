using System;

namespace Contest.Core.Helpers
{
    public static class CircleIntersectChecker
    {
        public static bool CheckIntersection(double X, double Y, double Radius, double X1, double Y1, double X2, double Y2)
        {
            double dx = X2 - X1;
            double dy = Y2 - Y1;
            double A = dx * dx + dy * dy;
            double B = 2 * (dx * (X1 - X) + dy * (Y1 - Y));
            double C = (X1 - X) * (X1 - X) +
                       (Y1 - Y) * (Y1 - Y) -
                       Radius * Radius;

            double discriminant = B * B - 4 * A * C;

            if (discriminant >= 0)
            {
                double t1 = (-B + (double)Math.Sqrt(discriminant)) / (2 * A);
                double t2 = (-B - (double)Math.Sqrt(discriminant)) / (2 * A);

                if ((t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1))
                {
                    return true;
                }
            }

            return false;
        }
    }
}