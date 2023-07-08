using Contest.Api;
using Contest.Core.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Contest.Core.Helpers
{
    public class ProblemRepository
    {
        private ContestApiClient _client;
        private string? _cacheFolder;

        public ProblemRepository(ContestApiClient client)
        {
            _client = client;
            _cacheFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\problems"));
        }

        public async Task<Problem> LoadProblem(int id)
        {
            var filePath = Path.Combine(_cacheFolder, $"problem_{id:D4}.json");
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<Problem>(File.ReadAllText(filePath));
            }

            var pr = await _client.GetProblem(id);

            var problem = new Problem(id, pr.room_width, pr.room_height, pr.stage_width, pr.stage_height, pr.stage_bottom_left,
                pr.musicians, pr.attendees);

            //if (Directory.Exists(_cacheFolder))
            //{
            //    await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(problem, Formatting.Indented));
            //}

            return problem;

            //var testproblem = new Problem()
            //{
            //    Id = 1,
            //    Width = 500,
            //    Height = 500,
            //    Stage = new Stage()
            //    {
            //        Width = 100,
            //        Height = 100,
            //        X = 200,
            //        Y = 0
            //    },
            //    Musicians = new[] { new Musician(0) },
            //    Attendees = new[]
            //    {
            //        new Attendee()
            //        {
            //            X = 250,
            //            Y = 300,
            //            Tastes = new[] { 1000.0 }
            //        }
            //    }
            //};

            //return testproblem;
        }

        public async Task SubmitSolution(Problem problem)
        {
            await _client.SubmitSolution(problem.Id, problem.Placements.Select(x => (x.X, x.Y)));
        }

        public async Task<double> GetCurrentScore(int id)
        {
            return await _client.GetProblemScore(id);
        }

        public async Task<int> GetNumberOfProblems()
        {
            return await _client.GetProblems();
        }
    }
}