using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Services;

/// <summary>
///     Service for writing YAML files.
/// </summary>
public class YamlWriteService
{
    
    private readonly IConfiguration _configuration;
    private readonly ILogger<YamlWriteService> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="YamlWriteService"/> class.
    /// </summary>
    public YamlWriteService(IConfiguration configuration, ILogger<YamlWriteService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with form in the config directory.
    /// </summary>
    public async Task WriteConfigFile(string yamlConfiguration, string fileName)
    {
        string configDirectory = _configuration.GetValue<string>("ConfigurationDirectory");
        if (!Directory.Exists(configDirectory))
        {
            Directory.CreateDirectory(configDirectory);
            _logger.LogDebug($"Directory '{configDirectory}' was created");
        }

        string filePath = Path.Combine(configDirectory, fileName);

        await WriteYamlFile(yamlConfiguration, filePath);
    }

    private async Task WriteYamlFile(string yamlConfiguration, string filePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize(new StringReader(yamlConfiguration));

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
