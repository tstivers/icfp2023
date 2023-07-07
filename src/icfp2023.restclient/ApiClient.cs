using icfp2023.Api.Models;
using icfp2023.restclient.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace icfp2023.Api
{
    public class ApiClient
    {
        private readonly RestClient _client;
        private readonly string _token;

        public ApiClient(string endpoint, string token)
        {
            _token = token;

            var options = new RestClientOptions(endpoint);
            var client = new RestClient(options);

            _client = client;
        }

        private async Task<T> HandleRequest<T>(RestRequest request)
        {
            request.AddHeader("Authorization", $"Bearer {_token}");

            var response = await _client.ExecuteAsync<T>(request) ?? throw new Exception();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ErrorMessage, response.ErrorException);
            }

            return response.Data;
        }

        private async Task<T> HandleStatusRequest<T>(RestRequest request)
        {
            var response = await HandleRequest<StatusResponse>(request);

            if (response.Failure != null)
                throw new Exception(response.Failure);

            return JsonConvert.DeserializeObject<T>(response.Success);
        }

        public async Task<int> GetProblems()
        {
            var request = new RestRequest("problems");

            var response = await HandleRequest<ProblemsResponse>(request);

            return response.number_of_problems;
        }

        public async Task<ProblemResponse> GetProblem(int id)
        {
            var request = new RestRequest("problem");

            request.AddParameter("problem_id", id);

            var response = await HandleStatusRequest<ProblemResponse>(request);

            return response;
        }
    }
}