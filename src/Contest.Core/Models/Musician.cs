namespace Contest.Core.Models
{
    public class Musician
    {
        public Musician()
        { }

        public Musician(int instrument)
        {
            Instrument = instrument;
        }

        public int Instrument { get; }
        public double BestScore { get; set; }
        public int Id { get; set; }
        public double ActualScore { get; set; }
        public double PotentialScore { get; set; }

        public int NumBlocked { get; set; }
    }
}