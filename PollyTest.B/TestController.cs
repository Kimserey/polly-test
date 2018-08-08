using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace PollyTest.B
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public TestController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("five-seconds")]
        public async Task<ActionResult<string>> FiveSeconds()
        {
            var value = await _cache.GetStringAsync("five-seconds");

            if (value == null)
            {
                await _cache.SetStringAsync("five-seconds", "1");
                Thread.Sleep(5000);
            }
            else if (value == "1")
            {
                await _cache.SetStringAsync("five-seconds", "2");
                Thread.Sleep(5000);
            }
            //Only succeed at the 3rd attempt

            return Ok();
        }

        [HttpGet("exception")]
        public ActionResult Exception()
        {
            throw new NotImplementedException("This is not implemented");
        }
    }
}
