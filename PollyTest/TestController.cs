using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyTest
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public TestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("timeout")]
        public async Task<ActionResult> GetTimeout()
        {
            var client = _clientFactory.CreateClient();
            var timeout = await client.GetAsync("http://localhost:5100/test/five-seconds");
            return Ok(timeout);
        }

        [HttpGet("retry")]
        public ActionResult GetRetry()
        {
            return Ok();
        }

        [HttpGet("circuit-breaker")]
        public ActionResult GetCircuitBreaker()
        {
            return Ok();
        }
    }
}
