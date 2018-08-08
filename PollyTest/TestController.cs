using Microsoft.AspNetCore.Mvc;

namespace PollyTest
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet("timeout")]
        public ActionResult GetTimeout()
        {
            return Ok();
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
