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
    }
}