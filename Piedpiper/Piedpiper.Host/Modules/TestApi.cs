using System;
using Microsoft.AspNetCore.Mvc;

namespace Piedpiper.Host.Modules
{
    [Route("/test")]
    public class TestApi : Controller
    {
        [HttpGet]
        public DateTime Get() => DateTime.Now;
    }
}
