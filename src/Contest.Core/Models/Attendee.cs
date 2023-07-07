namespace Contest.Core.Models
{
    public class Attendee
    {
        public Attendee(double x, double y, double[] tastes)
        {
            X = x;
            Y = y;
            Tastes = tastes;
        }

        public double X { get; }
        public double Y { get; }
        public double[] Tastes { get; }
    }
}