using Contest.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Helpers
{
    public class AttendeeQuadTreeNode
    {
        public AttendeeQuadTreeNode(double x, double y, double w, double h)
        {
            Extents = (x, y, w, h);
            Center = (x + (w / 2), y + (h / 2));
            Attendees = new List<Attendee>();
        }

        public (double x, double y, double width, double height) Extents { get; set; }
        public (double x, double y) Center { get; set; }
        public List<Attendee> Attendees { get; set; }

        private double[] _tastes;

        public double[] Tastes
        {
            get
            {
                if (_tastes == null)
                {
                    _tastes = new double[Attendees[0].Tastes.Length];

                    foreach (var a in Attendees)
                    {
                        for (int i = 0; i < a.Tastes.Length; i++)
                            _tastes[i] += a.Tastes[i];
                    }
                }

                return _tastes;
            }
        }

        public AttendeeQuadTreeNode[]? Children { get; set; }

        public void Split()
        {
            var nw = Extents.width / 2;
            var nh = Extents.height / 2;

            var ul = new AttendeeQuadTreeNode(Extents.x, Extents.y, nw, nh);
            var ur = new AttendeeQuadTreeNode(Extents.x + nw, Extents.y, nw, nh);
            var ll = new AttendeeQuadTreeNode(Extents.x, Extents.y + nh, nw, nh);
            var lr = new AttendeeQuadTreeNode(Extents.x + nw, Extents.y + nh, nw, nh);

            foreach (var a in Attendees)
            {
                ul.MaybeAddAttendee(a);
                ur.MaybeAddAttendee(a);
                ll.MaybeAddAttendee(a);
                lr.MaybeAddAttendee(a);
            }

            Children = new[] { ul, ur, ll, lr };

            Attendees = null;
            _tastes = null;
        }

        public void MaybeAddAttendee(Attendee a)
        {
            if (a.X >= Extents.x && a.X < Extents.x + Extents.width)
                if (a.Y >= Extents.y && a.Y < Extents.y + Extents.height)
                    Attendees.Add(a);
        }

        public int NodeCount()
        {
            return Children?.Sum(x => x.NodeCount()) ?? 1;
        }

        public int AttendeeCount()
        {
            return Children?.Sum(x => x.AttendeeCount()) ?? Attendees.Count();
        }

        public static IEnumerable<AttendeeQuadTreeNode> GetAllNodesRecursively(AttendeeQuadTreeNode subnode)
        {
            // Return the parent before its children
            if (subnode.Children == null)
                yield return subnode;
            else
                foreach (var node in subnode.Children)
                {
                    foreach (var n in GetAllNodesRecursively(node))
                    {
                        yield return n;
                    }
                }
        }
    }
}