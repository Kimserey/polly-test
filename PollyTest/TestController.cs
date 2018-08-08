using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyTest
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IApiClient _client;

        public TestController(IApiClient client)
        {
            _client = client;
        }

        [HttpGet("timeout")]
        public async Task<ActionResult> GetTimeout()
        {
            try
            {
                var timeout = await _client.GetTimeout();
                return Ok(timeout);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("retry")]
        public async Task<ActionResult> GetRetry()
        {
            var exception = await _client.GetException();
            return Ok(exception);
        }

        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var exception = await _client.GetNotFound();
            return Ok(exception);
        }


        [HttpGet("circuit-breaker")]
        public ActionResult GetCircuitBreaker()
        {
            return Ok();
        }
    }
}
