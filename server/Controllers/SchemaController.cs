using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using BatteryDevicesMaster.Server.Models;
using BatteryDevicesMaster.Server.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for working with YAML schema files.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SchemaController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SchemaSerializer _schemaSerializer;
    private readonly ILogger<SchemaSerializer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaController"/> class.
    /// </summary>
    public SchemaController(IConfiguration configuration, SchemaSerializer schemaSerializer,
        ILogger<SchemaSerializer> logger)
    {
        _configuration = configuration;
        _schemaSerializer = schemaSerializer;
        _logger = logger;
    }

    /// <summary>
    ///      Returns a schema that client needs to render
    /// </summary>
    /// <response code="200">Schema for a client</response>
    /// <response code="404">File not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<JsonResult> Get([FromQuery] string fileName)
    {
        try
        {
            // TODO(purposelessness): call SchemaSerializer.ReadSchemaFile and return schema
            return StatusCode(501, new ErrorResponse { Message = "Not implemented yet" });
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning($"Error getting schema for a client: {ex.Message}");
            return StatusCode(404, new ErrorResponse() { Message = $"{fileName} file not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting schema for a client: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error getting schema for a client: {ex.Message}" });
        }
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with configuration in the static directory.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Unsuccessful file write</response>
    [HttpPost("parameters")]
    [ProducesResponseType(typeof(TextMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostParameters([FromBody] YamlConfigurationBody body)
    {
        string fileName = _configuration.GetValue<string>("YamlParametersFileName");
        try
        {
            await _schemaSerializer.WriteSchemaFile(body.YamlConfiguration, fileName);

            _logger.LogDebug($"File {fileName} successfully written.");
            return StatusCode(200, new TextMessage { Message = $"File {fileName} successfully written." });
        }
        catch (YamlDotNet.Core.SemanticErrorException ex)
        {
            _logger.LogWarning($"Error writing {fileName}: {ex.Message}");
            return StatusCode(400, new ErrorResponse { Message = $"Error writing {fileName}: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error writing {fileName}: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error writing {fileName}: {ex.Message}" });
        }
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with form in the static directory.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Unsuccessful file write</response>
    [HttpPost("form")]
    [ProducesResponseType(typeof(TextMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostForm([FromBody] YamlConfigurationBody body)
    {
        string fileName = _configuration.GetValue<string>("YamlFormFileName");
        try
        {
            await _schemaSerializer.WriteSchemaFile(body.YamlConfiguration, fileName);

            _logger.LogDebug($"File {fileName} successfully written.");
            return StatusCode(200, new TextMessage { Message = $"File {fileName} successfully written." });
        }
        catch (YamlDotNet.Core.SemanticErrorException ex)
        {
            _logger.LogWarning($"Error writing {fileName}: {ex.Message}");
            return StatusCode(400, new ErrorResponse { Message = $"Error writing {fileName}: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error writing {fileName}: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error writing {fileName}: {ex.Message}" });
        }
    }

    /// <summary>
    ///      Returns dummy schema
    /// </summary>
    /// <response code="200">Dummy json schema</response>
    [HttpGet]
    [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
    public ActionResult<JsonResult> GetJsonSchema()
    {
        var dummySchema = new
        {
            schema = new
            {
                title = "A registration form",
                description = "A simple form example.",
                type = "object",
                required = new[] { "firstName", "lastName" },
                properties = new
                {
                    firstName = new { type = "string", title = "First name", @default = "Chuck" },
                    lastName = new { type = "string", title = "Last name" },
                    age = new { type = "integer", title = "Age" },
                    bio = new { type = "string", title = "Bio" },
                    password = new { type = "string", title = "Password", minLength = 3 },
                    telephone = new { type = "string", title = "Telephone", minLength = 10 }
                }
            },
            model = new
            {
                lastName = "Norris", age = 75, bio = "Roundhouse kicking asses since 1940", password = "noneed"
            }
        };
        return new JsonResult(dummySchema);
    }
}

/// <summary>
///     Yaml configuration request body
/// </summary>
public struct YamlConfigurationBody
{
    /// <summary>
    ///     Yaml configuration message
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "YamlConfiguration must not be empty")]
    public string YamlConfiguration { get; set; }
}
