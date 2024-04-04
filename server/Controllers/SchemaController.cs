using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using BatteryDevicesMaster.Server.Models;
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
    private readonly ILogger<SchemaController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaController"/> class.
    /// </summary>
    public SchemaController(IConfiguration configuration, ILogger<SchemaController> logger)
    {
        _configuration = configuration;
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
            var schemasDirectory = _configuration.GetValue<string>("SchemasDirectory") ??
                                   throw new InvalidOperationException("SchemasDirectory is null");
            var filePath = Path.Combine(schemasDirectory, fileName);
            if (!Directory.Exists(schemasDirectory) || !System.IO.File.Exists(filePath))
            {
                _logger.LogError($"{schemasDirectory} directory or {fileName} file not exist");
                return StatusCode(StatusCodes.Status404NotFound,
                    new ErrorResponse() { Message = $"{fileName} file not found" });
            }

            return StatusCode(StatusCodes.Status501NotImplemented,
                new ErrorResponse { Message = "Not implemented yet" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting schema for a client: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = $"Error getting schema for a client: {ex.Message}" });
        }
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file in the static directory.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Unsuccessful file write</response>
    [HttpPost]
    [ProducesResponseType(typeof(TextMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post([FromBody] YamlConfigurationBody body)
    {
        try
        {
            var schemasDirectory = _configuration.GetValue<string>("SchemasDirectory") ??
                                   throw new InvalidOperationException("SchemasDirectory is null");
            if (!Directory.Exists(schemasDirectory))
            {
                Directory.CreateDirectory(schemasDirectory);
                _logger.LogDebug($"Directory '{schemasDirectory}' was created");
            }

            const string fileName = "config.yaml";

            var filePath = Path.Combine(schemasDirectory, fileName);

            await WriteYamlFile(filePath, body.YamlConfiguration);

            _logger.LogDebug($"File {fileName} successfully written.");
            return Ok(new TextMessage { Message = $"File {fileName} successfully written." });
        }
        catch (YamlDotNet.Core.SemanticErrorException ex)
        {
            _logger.LogWarning($"Error writing the file: {ex.Message}");
            return StatusCode(StatusCodes.Status400BadRequest,
                new ErrorResponse { Message = $"Error writing the file: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error writing the file: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = $"Error writing the file: {ex.Message}" });
        }
    }

    private static async Task WriteYamlFile(string filePath, string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize(new StringReader(yamlContent));

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        await using var sw = new StreamWriter(filePath, false, Encoding.UTF8);
        serializer.Serialize(sw, yamlObject);
        await sw.FlushAsync();
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