namespace Contest.Api.Models
{
    public class PlacementRequest
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class PlacementsRequest
    {
        public int problem_id { get; set; }
        public string contents { get; set; }
    }

    public class SubmissionContents
    {
        public PlacementRequest[] placements { get; set; }
    }
}