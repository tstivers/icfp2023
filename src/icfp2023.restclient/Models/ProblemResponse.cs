namespace Contest.Api.Models
{
    public class AttendeeResponse
    {
        public double x { get; set; }
        public double y { get; set; }
        public double[] tastes { get; set; }
    }

    public class ProblemResponse
    {
        public double room_width { get; set; }
        public double room_height { get; set; }

        public double stage_width { get; set; }
        public double stage_height { get; set; }

        public double[] stage_bottom_left { get; set; }

        public int[] musicians { get; set; }

        public AttendeeResponse[] attendees { get; set; }
    }
}