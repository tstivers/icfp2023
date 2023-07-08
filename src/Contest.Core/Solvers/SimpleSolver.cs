using Contest.Core.Helpers;
using Contest.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Contest.Core.Solvers
{
    public class SimpleSolver
    {
        public readonly Problem Problem;

        public readonly double[,][] ScoreMatrix;

        public int w;
        public int h;
        public int NumInstruments;
        public int numRefinements { get; set; }
        public double InitialScore { get; set; }
        public double LastScore { get; set; }
        public double CurrentScore { get; set; }
        public int NumMusiciansProcessed { get; set; }

        public delegate void Notify(Object sender);

        public delegate void LogMessage(Object sender, string message);

        public Notify OnNotify;

        public LogMessage OnLogMessage;

        public double[] InvalidScores;

        public SimpleSolver(Problem problem, int width, int height)
        {
            Problem = problem;

            w = width;
            h = height;
            NumInstruments = Problem.Musicians.Select(x => x.Instrument).Distinct().Count();

            InvalidScores = new double[NumInstruments];
            for (int i = 0; i < NumInstruments; i++)
            {
                InvalidScores[i] = double.MinValue;
            }

            ScoreMatrix = new double[w, h][];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    ScoreMatrix[x, y] = new double[NumInstruments];
                }
        }

        public (double x, double y) MatrixPosToPos(int x, int y)
        {
            var st = Problem.Stage.Y + 10;
            var sb = Problem.Stage.Y + Problem.Stage.Height - 10;

            var sl = Problem.Stage.X + 10;
            var sr = Problem.Stage.X + Problem.Stage.Width - 10;

            var hs = (sb - st) / (h - 1);
            var ws = (sr - sl) / (w - 1);

            return ((x * ws) + sl, (y * hs) + st);
        }

        public void Solve()
        {
            numRefinements = 0;
            NumMusiciansProcessed = 0;

            PopulateScoreMatrix(false);
            OnNotify?.Invoke(this);

            CalculateBestScores();
            OnNotify?.Invoke(this);

            foreach (var m in Problem.Musicians.OrderByDescending(x => x.BestScore))
            {
                var pos = HighestScoringStagePos(m.Instrument);
                m.PotentialScore = pos.score;
                Problem.Placements[m.Id].X = pos.x;
                Problem.Placements[m.Id].Y = pos.y;
                RecalculateValidPlacements();
                NumMusiciansProcessed++;
            }
            OnNotify?.Invoke(this);

            CalculateScores();
            InitialScore = GetScore();

            OnNotify?.Invoke(this);
            do
            {
                numRefinements++;

                PopulateScoreMatrix(true);

                foreach (var m in Problem.Musicians)
                {
                    var hs = HighestScoringStagePos(m.Instrument);
                    m.BestScore = hs.score;
                    m.BestScoringPos = (hs.x, hs.y);
                }

                OnNotify?.Invoke(this);

                var mip = Problem.Musicians.Where(x => x.BestScore > x.ActualScore)
                    .OrderByDescending(x => x.BestScore - x.ActualScore).FirstOrDefault();

                if (mip == null)
                    break;

                Problem.Placements[mip.Id].X = mip.BestScoringPos.x;
                Problem.Placements[mip.Id].Y = mip.BestScoringPos.y;
                CalculateScores();
                LastScore = CurrentScore;
                CurrentScore = GetScore();

                OnNotify?.Invoke(this);
                //} while (scorediff > Math.Abs(InitialScore) * 0.01); // 1%
            } while (true);
        }

        private void CalculateBestScores()
        {
            for (var i = 0; i < Problem.Musicians.Length; i++)
            {
                var m = Problem.Musicians[i];
                m.Id = i;

                m.BestScore = double.MinValue;

                var inst = m.Instrument;

                for (int x = 0; x < w; ++x)
                    for (int y = 0; y < h; ++y)
                    {
                        m.BestScore = Math.Max(m.BestScore, ScoreMatrix[x, y][inst]);
                    }
            }
        }

        private void RecalculateValidPlacements()
        {
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; ++x)
                {
                    var scores = ScoreMatrix[x, y];
                    var pos = MatrixPosToPos(x, y);

                    if (!isValidPlacement(pos.x, pos.y))
                        for (int m = 0; m < NumInstruments; m++)
                        {
                            scores[m] = double.MinValue;
                        }
                }
            });
        }

        private void CalculateScores()
        {
            Parallel.ForEach(Problem.Musicians, m =>
            {
                m.ActualScore = 0;
                m.NumBlocked = 0;
                var pos = Problem.Placements[m.Id];

                foreach (var a in Problem.Attendees)
                {
                    if (a.Tastes[m.Instrument] == 0)
                        continue;

                    // check for intersection
                    bool intersects = false;
                    foreach (var p in Problem.Placements)
                    {
                        if (p == pos)
                            continue;

                        if (CircleIntersectChecker.CheckIntersection(p.X, p.Y, 5.0, a.X, a.Y, pos.X, pos.Y))
                        {
                            intersects = true;
                            break;
                        }
                    }

                    if (intersects)
                    {
                        m.NumBlocked++;
                        continue;
                    }

                    var distance = this.distance(a.X, a.Y, pos.X, pos.Y);
                    m.ActualScore += 1000000 * a.Tastes[m.Instrument] / Math.Pow(distance, 2);
                }
            });
        }

        public double GetScore()
        {
            return Problem.Musicians.Sum(m => m.ActualScore);
        }

        public void PopulateScoreMatrix(bool checkIntersections)
        {
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; ++x)
                {
                    var scores = ScoreMatrix[x, y];

                    var pos = MatrixPosToPos(x, y);

                    ScoreMatrix[x, y] = ScorePlacement(pos.x, pos.y, checkIntersections);
                }
            });
        }

        public (double x, double y, double score) HighestScoringStagePos(int instrument)
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

            var tpos = MatrixPosToPos(pos.x, pos.y);

            return (tpos.x, tpos.y, highest);
        }

        public bool isValidPlacement(double x, double y)
        {
            foreach (var p in Problem.Placements.Where(x => x.X != 0 || x.Y != 0))
                if (distance(x, y, p.X, p.Y) < 10)
                    return false;

            return true;
        }

        public double distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public double[] ScorePlacement(double x, double y, bool checkIntersections)
        {
            // check if this is a valid placement
            if (!isValidPlacement(x, y))
            {
                return InvalidScores;
            }

            var scores = new double[NumInstruments];

            foreach (var a in Problem.Attendees)
            {
                bool intersects = false;

                if (checkIntersections)
                {
                    foreach (var p in Problem.Placements.Where(x => x.X != 0 || x.Y != 0))
                    {
                        if (CircleIntersectChecker.CheckIntersection(p.X, p.Y, 5.0, a.X, a.Y, x, y))
                        {
                            intersects = true;
                            break;
                        }
                    }
                }

                if (intersects)
                    continue;

                var distance = this.distance(a.X, a.Y, x, y);

                for (int i = 0; i < NumInstruments; i++)
                {
                    scores[i] += 1000000 * a.Tastes[i] / Math.Pow(distance, 2);
                }
            }

            return scores;
        }
    }
}