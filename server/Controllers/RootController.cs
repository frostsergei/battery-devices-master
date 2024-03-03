using Microsoft.AspNetCore.Mvc;

namespace BatteryDevicesMaster.Controllers;

/// <summary>
///     Controller for root path '/'
/// </summary>
[ApiController]
[Route("")]
public class RootController : ControllerBase
{
    /// <summary>
    ///     Redirects to an OpenAPI specification page
    /// </summary>
    /// <response code="300">Successful redirection</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status300MultipleChoices)]
    public ActionResult Swagger()
    {
        return new RedirectResult("/swagger");
    }

    /// <summary>
    ///     Check server health
    /// </summary>
    /// <response code="200">Server is healthy</response>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Ping()
    {
        return Ok();
    }
}