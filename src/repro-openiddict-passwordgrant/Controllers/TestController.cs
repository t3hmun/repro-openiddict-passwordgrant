using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string Hello()
        {
            return "hello";
        }

    }
}
