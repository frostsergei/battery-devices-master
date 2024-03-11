using Microsoft.AspNetCore.Mvc;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for '/ping' path
/// </summary>
[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    /// <summary>
    ///     Check server health
    /// </summary>
    /// <response code="200">Server is healthy</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Ping()
    {
        return Ok();
    }
}