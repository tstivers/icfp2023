﻿using Contest.Core.Helpers;
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

        public SimpleSolver(Problem problem, int width, int height)
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
        }

        public void Solve()
        {
            numRefinements = 0;

            PopulateScoreMatrix(false);

            CalculateBestScores();

            foreach (var m in Problem.Musicians.OrderByDescending(x => x.BestScore))
            {
                var pos = HighestScoringStagePos(m.Instrument);
                m.PotentialScore = pos.score;
                Problem.Placements[m.Id].X = pos.x;
                Problem.Placements[m.Id].Y = pos.y;
                RecalculateValidPlacements();
            }

            double scorediff;
            double score;

            CalculateScores();
            InitialScore = GetScore();

            do
            {
                numRefinements++;
                CalculateScores();
                score = GetScore();
                var baddies = Problem.Musicians.OrderByDescending(x => x.PotentialScore - x.ActualScore).ToList();
                PopulateScoreMatrix(true);

                var bad = baddies.First();
                var newpos = HighestScoringStagePos(bad.Instrument);
                if (newpos.score > bad.ActualScore)
                {
                    var oldscore = GetScore();
                    Problem.Placements[bad.Id].X = newpos.x;
                    Problem.Placements[bad.Id].Y = newpos.y;
                    CalculateScores();
                    scorediff = GetScore() - oldscore;
                }
                else
                {
                    scorediff = 0;
                }
            } while (scorediff > Math.Abs(score) * 0.01); // 1%
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

                    double xPos = ((Problem.Stage.Width / w) * x) + Problem.Stage.X;
                    double yPos = ((Problem.Stage.Height / h) * y) + Problem.Stage.Y;

                    if (!isValidPlacement(xPos, yPos))
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

                    double xPos = ((Problem.Stage.Width / w) * x) + Problem.Stage.X;
                    double yPos = ((Problem.Stage.Height / h) * y) + Problem.Stage.Y;

                    for (int m = 0; m < NumInstruments; m++)
                    {
                        scores[m] = ScorePlacement(m, xPos, yPos, checkIntersections);
                    }
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

            double xPos = ((Problem.Stage.Width / w) * pos.x) + Problem.Stage.X;
            double yPos = ((Problem.Stage.Height / h) * pos.y) + Problem.Stage.Y;

            return (xPos, yPos, highest);
        }

        public bool isValidPlacement(double x, double y)
        {
            if (x - 10 < Problem.Stage.X || x + 10 > Problem.Stage.X + Problem.Stage.Width ||
                y - 10 < Problem.Stage.Y || y + 10 > Problem.Stage.Y + Problem.Stage.Height)
                return false;

            foreach (var p in Problem.Placements.Where(x => x.X != 0 || x.Y != 0))
                if (distance(x, y, p.X, p.Y) < 10)
                    return false;

            return true;
        }

        public double distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public double ScorePlacement(int instrument, double x, double y, bool checkIntersections)
        {
            // check if this is a valid placement
            if (!isValidPlacement(x, y))
                return double.MinValue;

            double score = 0;

            foreach (var a in Problem.Attendees)
            {
                bool intersects = false;

                if (checkIntersections)
                {
                    foreach (var p in Problem.Placements)
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

                var distance = Math.Sqrt(Math.Pow(a.X - x, 2) + Math.Pow(a.Y - y, 2));
                score += 1000000 * a.Tastes[instrument] / Math.Pow(distance, 2);
            }

            return score;
        }
    }
}