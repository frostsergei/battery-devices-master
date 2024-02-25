using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [ApiController]
    [Route("ping")]
    public class DummyController : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Ping()
        {
            return new JsonResult("Hello World!");
        }
    }
}
