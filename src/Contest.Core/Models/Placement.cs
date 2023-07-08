namespace Contest.Core.Models
{
    public struct Placement
    {
        public double X { get; set; }
        public double Y { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Placement placement &&
                   X == placement.X &&
                   Y == placement.Y;
        }

        public static bool operator ==(Placement left, Placement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Placement left, Placement right)
        {
            return !(left == right);
        }
    }
}