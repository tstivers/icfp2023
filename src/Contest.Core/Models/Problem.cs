using Contest.Api.Models;
using System.Linq;

namespace Contest.Core.Models
{
    public class Problem
    {
        public Problem()
        {
        }

        public Problem(int id, double width, double height, double stageWidth, double stageHeight, double[] stagePos, int[] musicians, AttendeeResponse[] attendees)
        {
            Id = id;
            Width = width;
            Height = height;

            Stage = new Stage(stageWidth, stageHeight, stagePos);
            Musicians = musicians.Select(x => new Musician(x)).ToArray();
            Attendees = attendees.Select(x => new Attendee(x.x, x.y, x.tastes)).ToArray();
        }

        public int Id { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public Stage Stage { get; set; }

        public Musician[] Musicians { get; set; }
        public Attendee[] Attendees { get; set; }

        private Placement[]? _placements;

        public Placement[] Placements
        {
            get
            {
                _placements ??= new Placement[Musicians.Length];

                return _placements;
            }
            set
            {
                _placements = value;
            }
        }
    }
}