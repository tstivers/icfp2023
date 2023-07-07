using Contest.Core.Models;
using icfp2023.Api;
using System;
using System.Threading.Tasks;

namespace Contest.Core.Helpers
{
    public class ProblemRepository
    {
        private ApiClient _client;

        public ProblemRepository(ApiClient client)
        { _client = client; }

        public async Task<Problem> LoadProblem(int id)
        {
            var pr = await _client.GetProblem(id);

            return new Problem(id, pr.room_width, pr.room_height, pr.stage_width, pr.stage_height, pr.stage_bottom_left,
                pr.musicians, pr.attendees);
        }

        public async Task SubmitSolution(Problem problem)
        {
            throw new NotImplementedException();
        }

        public async Task<double> GetCurrentScore(int id)
        {
            throw new NotImplementedException();
        }
    }
}