namespace Contest.Core.Models
{
    public class Attendee
    {
        public Attendee()
        { }

        public Attendee(double x, double y, double[] tastes)
        {
            X = x;
            Y = y;
            Tastes = tastes;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double[] Tastes { get; set; }
    }
}