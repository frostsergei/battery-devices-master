using System.ComponentModel.DataAnnotations;
using BatteryDevicesMaster.Server.Models;
using BatteryDevicesMaster.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for working with parse JSON user form result to XML db.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class JsonFormController : ControllerBase
{
    private readonly XmlWriteService _xmlWriteService;
    private readonly ILogger<YamlWriteService> _logger;
    private const string FileName = "output.xdb";

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFormController"/> class.
    /// </summary>
    public JsonFormController(XmlWriteService xmlWriteService, ILogger<YamlWriteService> logger)
    {
        _xmlWriteService = xmlWriteService;
        _logger = logger;
    }

    /// <summary>
    ///     Writes the content of JSON string to XML db file.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Unsuccessful file write</response>
    [HttpPost]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public IActionResult Post([FromBody] JsonForm jsonForm)
    {
        try
        {
            _xmlWriteService.WriteXml(jsonForm.Form, FileName);
            _logger.LogDebug($"File {FileName} successfully written.");
            var fileStream = System.IO.File.OpenRead(_xmlWriteService.GetPathToXmlFile(FileName));
            return new FileStreamResult(fileStream, "application/octet-stream");
        }
        catch (JsonReaderException ex)
        {
            _logger.LogWarning($"Error writing {FileName}: {ex.Message}");
            return StatusCode(400, new ErrorResponse { Message = $"Error writing {FileName}: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error writing {FileName}: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error writing {FileName}: {ex.Message}" });
        }
    }
}

/// <summary>
///     JSON form configuration request body
/// </summary>
public struct JsonForm
{
    /// <summary>
    ///     Message with JSON form
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "YamlConfiguration must not be empty")]
    public string Form { get; set; }
}
