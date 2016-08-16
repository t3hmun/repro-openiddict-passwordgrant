using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace repro_openiddict_passwordgrant.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "hello";
        }

        // GET api/values
        [HttpGet("[action]")]
        public async Task<string> Hello()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:55542/api/test");
                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
        }

    }
}
