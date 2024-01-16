using Freelancers.Models.Domain;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Net.Http.Headers;
using System.Text;

namespace Freelancers.Repository
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private const string apiTokenUrl = "http://localhost:7041/api/Token";
        private const string apiBaseUrl = "http://localhost:7041/api/Freelancers";
       
        public UserService()
        {
            httpClient = new HttpClient();
        }


        public async Task<TokenResponse> GetAccessToken()
        {
            var tokenResponse = new TokenResponse();

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiTokenUrl);
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonResult = await response.Content.ReadAsStringAsync();
                tokenResponse.Token = jsonResult;
            }
            catch (Exception ex)
            {


            }
            return tokenResponse;
        }



        public async Task<IEnumerable<FreelancerViewModel>> GetFreelancers(string token)
        {
 
            var request = new HttpRequestMessage(HttpMethod.Get, apiBaseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<FreelancerViewModel[]>(await response.Content.ReadAsStringAsync());
        }

        public async Task<FreelancerViewModel> AddFreelancer(FreelancerViewModel newFreelancer, string token)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Access-Control-Request-Headers", "authorization,content-type");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var postData = JsonConvert.SerializeObject(newFreelancer);
            var jsonContent = new StringContent(postData, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiBaseUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FreelancerViewModel>(stringData);
            }
            else
            {
                throw new ArgumentException("something bad happended");
            }

        }


        public async Task<string> UpdateFreelancer(int id, FreelancerViewModel updateFreelancer)
        {
            TokenResponse token_response = await GetAccessToken();

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Access-Control-Request-Headers", "authorization,content-type");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_response.Token.ToString());

            var putData = JsonConvert.SerializeObject(updateFreelancer);
            var jsonContent = new StringContent(putData, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{apiBaseUrl}/{id}", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new ArgumentException("something bad happended");
            }
        }

        public async Task<string> DeleteFreelancer(int id)
        {
            TokenResponse token_response = await GetAccessToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_response.Token.ToString());
            var response = await httpClient.DeleteAsync($"{apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }
            else
            {
                throw new ArgumentException("something bad happended");
            }
        }

        public async Task<FreelancerViewModel> GetFreelancerById(int id, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync($"{apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FreelancerViewModel>(stringData);

            }
            else
            {
                throw new ArgumentException("something bad happended");
            }
        }




    }
}
