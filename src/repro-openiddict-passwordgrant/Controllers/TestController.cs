using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace repro_openiddict_passwordgrant.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private const string Localhost = "http://localhost:50410/";

        [HttpGet]
        public string Get()
        {
            return "hello";
        }

        [HttpGet("[action]")]
        public async Task<string> Hello()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{Localhost}api/test");
                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
        }


        // GET api/values
        [HttpGet("[action]")]
        public async Task<string> Token()
        {
            using (var client = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", "testuser"},
                    {"password", "1234abcABC!!"}
                };

                var encodedContent = new FormUrlEncodedContent(parameters);

                var response = await client.PostAsync($"{Localhost}auth/token", encodedContent);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}