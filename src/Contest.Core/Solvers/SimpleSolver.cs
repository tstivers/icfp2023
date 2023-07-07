using Contest.Core.Models;
using System;

namespace Contest.Core.Solvers
{
    public class SimpleSolver
    {
        public readonly Problem Problem;

        public readonly double[,][] ScoreMatrix;

        public int w;
        public int h;

        public SimpleSolver(Problem problem, int width, int height)
        {
            Problem = problem;
            w = width;
            h = height;

            ScoreMatrix = new double[w, h][];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    ScoreMatrix[x, y] = new double[Problem.Musicians.Length];
                }
        }

        public void Solve()
        {
            // generate x * x matrix for stage
            // calculate score for each cell
            // for each musician
            // place in highest scoring valid cell
            // recalc score matrix
        }

        public void CalculateScores()
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var scores = ScoreMatrix[x, y];

                    double xStagePos = (Problem.Stage.Width / w) * x;
                    double yStagePos = (Problem.Stage.Height / h) * y;

                    for (int m = 0; m < Problem.Musicians.Length; m++)
                    {
                        scores[m] = ScorePlacement(Problem.Musicians[m].Instrument, xStagePos, yStagePos);
                    }
                }
        }

        public double ScorePlacement(int instrument, double x, double y)
        {
            double score = 0;
            foreach (var a in Problem.Attendees)
            {
                var distance = Math.Sqrt(Math.Pow(a.X - x, 2) + Math.Pow(a.Y - y, 2));
                score += 1000000 * a.Tastes[instrument] / Math.Pow(distance, 2);
            }

            return score;
        }
    }
}