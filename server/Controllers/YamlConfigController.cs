using System.ComponentModel.DataAnnotations;
using BatteryDevicesMaster.Server.Models;
using BatteryDevicesMaster.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for working with YAML files.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class YamlConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly YamlWriteService _yamlWriteService;
    private readonly ILogger<YamlWriteService> _logger;
    /// <summary>
    /// Initializes a new instance of the <see cref="YamlConfigController"/> class.
    /// </summary>
    public YamlConfigController(IConfiguration configuration, YamlWriteService yamlWriteService,
        ILogger<YamlWriteService> logger)
    {
        _configuration = configuration;
        _yamlWriteService = yamlWriteService;
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
        string fileName = _configuration.GetValue<string>("YamlParametersFileName");
        try
        {
            await _yamlWriteService.WriteConfigFile(body.YamlConfiguration, fileName);

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
            await _yamlWriteService.WriteConfigFile(body.YamlConfiguration, fileName);

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
