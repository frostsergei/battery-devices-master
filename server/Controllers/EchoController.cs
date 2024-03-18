using System.ComponentModel.DataAnnotations;
using BatteryDevicesMaster.Server.Models;
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
    [ProducesResponseType(typeof(TextMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<TextMessage> Echo([FromBody] TextMessage body)
    {
        Console.WriteLine($"Message {body.Message}");
        if (body.Message.Contains("bad", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new ErrorResponse { Message = "Bad word in request" });
        }

        return Ok(new TextMessage { Message = body.Message });
    }
}