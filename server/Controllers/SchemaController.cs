using System.ComponentModel.DataAnnotations;
using BatteryDevicesMaster.Server.Models;
using BatteryDevicesMaster.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for working with YAML schema files.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SchemaController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly XmlSerializer _xmlSerializer;
    private readonly SchemaSerializer _schemaSerializer;
    private readonly ILogger<SchemaController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaController"/> class.
    /// </summary>
    public SchemaController(
        IConfiguration configuration,
        SchemaSerializer schemaSerializer,
        XmlSerializer xmlSerializer,
        ILogger<SchemaController> logger)
    {
        _configuration = configuration;
        _schemaSerializer = schemaSerializer;
        _xmlSerializer = xmlSerializer;
        _logger = logger;
    }

    /// <summary>
    ///      Returns a schema that client needs to render
    /// </summary>
    /// <response code="200">Schema for a client</response>
    /// <response code="404">File not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> Get([FromQuery] string fileName)
    {
        try
        {
            var result = await _schemaSerializer.ReadSchema(fileName);
            return result;
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
    ///     Writes the content of database JSON to XML file.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("database")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public IActionResult Post([FromBody] Database database)
    {
        try
        {
            const string fileName = "output.xdb";
            _xmlSerializer.WriteXml(database.Content, fileName);
            var fileStream = System.IO.File.OpenRead(_xmlSerializer.GetPathToXmlFile(fileName));
            return new FileStreamResult(fileStream, "application/octet-stream");
        }
        catch (JsonReaderException ex)
        {
            _logger.LogWarning($"Error writing database: {ex.Message}");
            return StatusCode(400, new ErrorResponse { Message = $"Error writing database: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error writing database: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error writing database: {ex.Message}" });
        }
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with configuration in the static directory.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Unsuccessful file write</response>
    [HttpPost("parameters")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostParameters([FromBody] Schema body)
    {
        string fileName = _configuration.GetValue<string>("YamlParametersFileName");
        try
        {
            await _schemaSerializer.WriteSchema(body.Content, fileName);

            _logger.LogDebug($"File {fileName} successfully written");
            return Ok();
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
    /// Get parameters file
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("parameters")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public IActionResult GetParameters()
    {
        try
        {
            var schemasDirectory = _configuration.GetValue<string>("SchemasDirectory") ??
                                   throw new InvalidOperationException("SchemasDirectory is null");
            var parameterFilename = _configuration.GetValue<string>("YamlParametersFileName") ??
                                    throw new InvalidOperationException("YamlParametersFileName is null");
            var fileStream = System.IO.File.OpenRead(Path.Combine(schemasDirectory, parameterFilename));
            return new FileStreamResult(fileStream, "application/octet-stream");
        }
        catch (FileNotFoundException ex)
        {
            return StatusCode(StatusCodes.Status404NotFound,
                new ErrorResponse() { Message = $"Error getting parameters: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting parameters: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error getting parameters: {ex.Message}" });
        }
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with form in the static directory.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("form")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostForm([FromBody] Schema body)
    {
        string fileName = _configuration.GetValue<string>("YamlFormFileName");
        try
        {
            await _schemaSerializer.WriteSchema(body.Content, fileName);

            _logger.LogDebug($"File {fileName} successfully written");
            return Ok();
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
    [HttpGet("dummy")]
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
                lastName = "Norris",
                age = 75,
                bio = "Roundhouse kicking asses since 1940",
                password = "noneed"
            }
        };
        return new JsonResult(dummySchema);
    }
}

/// <summary>
///     Yaml configuration request body
/// </summary>
public struct Schema
{
    /// <summary>
    ///     Yaml configuration message
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Content must not be empty")]
    public string Content { get; set; }
}

/// <summary>
///     JSON form configuration request body
/// </summary>
public struct Database
{
    /// <summary>
    ///     Message with JSON form
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Content must not be empty")]
    public string Content { get; set; }
}