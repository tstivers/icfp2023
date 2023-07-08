using Contest.Api;
using Contest.Core.Helpers;

namespace Contest.cli
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var client = new ContestApiClient("https://api.icfpcontest.com", "***REMOVED***");
            var repo = new ProblemRepository(client);

            //Console.WriteLine(await client.GetProblems());

            var problem1 = await repo.LoadProblem(1);

            Console.WriteLine($"number of musicians: {problem1.Musicians.Length}");
            Console.WriteLine($"unique instruments: {problem1.Musicians.Select(x => x.Instrument).Distinct().Count()}");
        }
    }
}