using Contest.Core.Helpers;
using Contest.Core.Models;
using MoreLinq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Contest.Core.Solvers
{
    public class QuadtreeSolver
    {
        public readonly Problem Problem;

        public readonly double[,][] ScoreMatrix;

        public int w;
        public int h;
        public int NumInstruments;

        public AttendeeQuadTreeNode AttendeeQuadTree { get; set; }

        public QuadtreeSolver(Problem problem, int width, int height, int minNodeCount)
        {
            Problem = problem;
            w = width;
            h = height;
            NumInstruments = Problem.Musicians.Select(x => x.Instrument).Distinct().Count();

            ScoreMatrix = new double[w, h][];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    ScoreMatrix[x, y] = new double[NumInstruments];
                }

            AttendeeQuadTree = new AttendeeQuadTreeNode(0, 0, Problem.Width, Problem.Height);

            foreach (var a in Problem.Attendees)
                AttendeeQuadTree.MaybeAddAttendee(a);

            Assert.That(AttendeeQuadTree.AttendeeCount(), Is.EqualTo(Problem.Attendees.Length));
            Assert.That(Problem.Attendees.Length, Is.GreaterThan(minNodeCount));

            while (AttendeeQuadTree.NodeCount() < minNodeCount)
            {
                var biggestLeaf = AttendeeQuadTreeNode.GetAllNodesRecursively(AttendeeQuadTree)
                    .MaxBy(x => x.Attendees.Count).First();
                biggestLeaf.Split();
            }

            Assert.That(AttendeeQuadTree.AttendeeCount(), Is.EqualTo(Problem.Attendees.Length)); // don't lose attendees
        }

        public void Solve()
        {
            // generate x * x matrix for stage
            // calculate score for each cell
            // for each musician
            CalculateScores();
            for (int i = 0; i < Problem.Musicians.Length; i++)
            {
                var pos = HighestScoringStagePos(Problem.Musicians[i].Instrument);
                Problem.Placements[i].X = pos.x;
                Problem.Placements[i].Y = pos.y;
            }
            // place in highest scoring valid cell
            // recalc score matrix
        }

        public void CalculateScores()
        {
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; ++x)
                {
                    var scores = ScoreMatrix[x, y];

                    double xPos = ((Problem.Stage.Width / w) * x) + Problem.Stage.X;
                    double yPos = ((Problem.Stage.Height / h) * y) + Problem.Stage.Y;

                    for (int m = 0; m < NumInstruments; m++)
                    {
                        scores[m] = ScorePlacement(m, xPos, yPos);
                    }
                }
            });
        }

        public (double x, double y) HighestScoringStagePos(int instrument)
        {
            double highest = double.MinValue;
            (int x, int y) pos = (-1, -1);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    if (ScoreMatrix[x, y][instrument] > highest)
                    {
                        highest = ScoreMatrix[x, y][instrument];
                        pos = (x, y);
                    }
                }

            if (pos == (-1, -1))
            {
                throw new Exception("couldn't find a valid spot");
            }

            double xPos = ((Problem.Stage.Width / w) * pos.x) + Problem.Stage.X;
            double yPos = ((Problem.Stage.Height / h) * pos.y) + Problem.Stage.Y;

            return (xPos, yPos);
        }

        public bool isValidPlacement(double x, double y)
        {
            if (x - 10 < Problem.Stage.X || x + 10 > Problem.Stage.X + Problem.Stage.Width ||
                y - 10 < Problem.Stage.Y || y + 10 > Problem.Stage.Y + Problem.Stage.Height)
                return false;

            foreach (var p in Problem.Placements.Where(x => x.X != 0 || x.Y != 0))
                if (GetDistance(x, y, p.X, p.Y) < 10)
                    return false;

            return true;
        }

        public double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public double ScorePlacement(int instrument, double x, double y)
        {
            // check if this is a valid placement
            if (!isValidPlacement(x, y))
                return double.MinValue;

            double score = 0;

            //foreach (var a in Problem.Attendees)
            //{
            //    var distance = Math.Sqrt(Math.Pow(a.X - x, 2) + Math.Pow(a.Y - y, 2));
            //    score += 1000000 * a.Tastes[instrument] / Math.Pow(distance, 2);
            //}

            foreach (var n in AttendeeQuadTreeNode.GetAllNodesRecursively(AttendeeQuadTree).Where(x => x.Attendees.Count() != 0))
            {
                var distance = this.GetDistance(n.Center.x, n.Center.y, x, y);
                score += 1000000 * n.Tastes[instrument] / Math.Pow(distance, 2);
            }

            return score;
        }
    }
}