using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Services;

/// <summary>
///     Service for serializing and deserializing schema files
/// </summary>
public class SchemaSerializer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SchemaSerializer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaSerializer"/> class.
    /// </summary>
    public SchemaSerializer(IConfiguration configuration, ILogger<SchemaSerializer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    ///     Returns parsed schema content
    /// </summary>
    /// <param name="fileName">Name of file in schemas directory</param>
    /// <exception cref="InvalidOperationException">Server configuration is incomplete</exception>
    /// <exception cref="FileNotFoundException">Directory/file not found</exception>
    public async Task ReadSchemaFile(string fileName)
    {
        var schemasDirectory = _configuration.GetValue<string>("SchemasDirectory") ??
                               throw new InvalidOperationException("SchemasDirectory is null");
        var filePath = Path.Combine(schemasDirectory, fileName);
        if (!Directory.Exists(schemasDirectory) || !File.Exists(filePath))
        {
            throw new FileNotFoundException($"{fileName} not found in {schemasDirectory}");
        }

        // TODO(purposelessness): read file content and parse
        return;
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with form in the schema directory.
    /// </summary>
    public async Task WriteSchemaFile(string content, string fileName)
    {
        string configDirectory = _configuration.GetValue<string>("ConfigurationDirectory");
        if (!Directory.Exists(configDirectory))
        {
            Directory.CreateDirectory(configDirectory);
            _logger.LogDebug($"Directory '{configDirectory}' was created");
        }

        string filePath = Path.Combine(configDirectory, fileName);

        await WriteYamlFile(content, filePath);
    }

    private static async Task WriteYamlFile(string content, string filePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize(new StringReader(content));

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        await using StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
        serializer.Serialize(sw, yamlObject);
        await sw.FlushAsync();
    }
}
