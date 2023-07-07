using icfp2023.restclient.Models;
using System.Linq;

namespace Contest.Core.Models
{
    public class Problem
    {
        public Problem(int id, double width, double height, double stageWidth, double stageHeight, double[] stagePos, int[] musicians, AttendeeResponse[] attendees)
        {
            Id = id;
            Width = width;
            Height = height;

            Stage = new Stage(stageWidth, stageHeight, stagePos);
            Musicians = musicians.Select(x => new Musician(x)).ToArray();
            Attendees = attendees.Select(x => new Attendee(x.x, x.y, x.tastes)).ToArray();
            Placements = new Placement[Musicians.Length];
        }

        public int Id { get; }

        public double Width { get; }
        public double Height { get; }

        public Stage Stage { get; }

        public Musician[] Musicians { get; }
        public Attendee[] Attendees { get; }
        public Placement[] Placements { get; }
    }
}