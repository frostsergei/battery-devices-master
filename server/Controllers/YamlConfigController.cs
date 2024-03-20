using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using BatteryDevicesMaster.Server.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for working with YAML files.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class YamlConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<YamlConfigController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="YamlConfigController"/> class.
    /// </summary>
    public YamlConfigController(IConfiguration configuration, ILogger<YamlConfigController> logger)
    {
        _configuration = configuration;
        _logger = logger;
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
        return await WriteConfigFile(body.YamlConfiguration, "parameters.yaml");
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
        return await WriteConfigFile(body.YamlConfiguration, "form.yaml");
    }

    private async Task WriteYamlFile(string filePath, string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize(new StringReader(yamlContent));

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            serializer.Serialize(sw, yamlObject);
            await sw.FlushAsync();
        }
    }

    private async Task<IActionResult> WriteConfigFile(string yamlConfiguration, string fileName)
    {
        try
        {
            string configDirectory = _configuration.GetValue<string>("ConfigurationDirectory");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
                _logger.LogDebug($"Directory '{configDirectory}' was created");
            }

            string filePath = Path.Combine(configDirectory, fileName);

            await WriteYamlFile(filePath, yamlConfiguration);

            _logger.LogDebug($"File {fileName} successfully written.");
            return Ok(new TextMessage { Message = $"File {fileName} successfully written." });
        }
        catch (YamlDotNet.Core.SemanticErrorException ex)
        {
            _logger.LogWarning($"Error writing {fileName}: {ex.Message}");
            return BadRequest(new ErrorResponse { Message = $"Error writing {fileName}: {ex.Message}" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error writing {fileName}: {ex.Message}");
            return StatusCode(500, new ErrorResponse { Message = $"Error writing {fileName}: {ex.Message}" });
        }
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