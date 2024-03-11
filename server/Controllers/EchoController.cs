using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for '/ping' path
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class EchoController : ControllerBase
{
    /// <summary>
    ///     Echo-server handler
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    [HttpPost]
    [ProducesResponseType(typeof(EchoBody), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<EchoBody> Echo([FromBody] EchoBody body)
    {
        Console.WriteLine($"Message {body.Message}");
        if (body.Message.Contains("bad", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new ErrorResponse { Message = "Bad word in request" });
        }

        return Ok(new EchoBody { Message = body.Message });
    }
}

/// <summary>
///     Echo request/response body
/// </summary>
public struct EchoBody
{
    /// <summary>
    ///     Echo message
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Message must not be empty")]
    public string Message { get; set; }
}

/// <summary>
///     Error response with message
/// </summary>
public struct ErrorResponse
{
    /// <summary>
    ///     Error message
    /// </summary>
    [Required]
    public string Message { get; set; }
}