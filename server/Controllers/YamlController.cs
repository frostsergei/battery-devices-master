using Microsoft.AspNetCore.Mvc;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for working with YAML files.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class YamlController : ControllerBase
{
    private static readonly string ConfigDirectory = Path.Combine("Config");
    
    /// <summary>
    ///     Writes the content of a YAML string to a YAML file in the static directory.
    /// </summary>
    /// <response code="200">Request message</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Unsuccessful file write</response>
    [HttpPost]
    [ProducesResponseType(typeof(EchoBody), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] EchoBody body)
    {
        try
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            string fileName = "config.yaml";

            string filePath = Path.Combine(ConfigDirectory, fileName);

            await WriteYamlFile(filePath, body.Message);

            return Ok(new EchoBody { Message = $"File {fileName} successfully written." });
        }
        catch (YamlDotNet.Core.SemanticErrorException ex)
        {
            return BadRequest(new ErrorResponse { Message = $"Error writing the file: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = $"Error writing the file: {ex.Message}" });
        }
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
}
