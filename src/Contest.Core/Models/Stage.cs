namespace Contest.Core.Models
{
    public class Stage
    {
        public Stage(double width, double height, double[] pos)
        {
            Width = width;
            Height = height;
            X = pos[0]; Y = pos[1];
        }

        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}