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
    private readonly ParameterSchemaParser _parameterSchemaParser;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaSerializer"/> class.
    /// </summary>
    public SchemaSerializer(IConfiguration configuration, ILogger<SchemaSerializer> logger, ParameterSchemaParser parameterSchemaParser)
    {
        _configuration = configuration;
        _logger = logger;
        _parameterSchemaParser = parameterSchemaParser;
    }

    /// <summary>
    ///     Returns parsed schema content
    /// </summary>
    /// <param name="fileName">Name of file in schemas directory</param>
    /// <exception cref="InvalidOperationException">Server configuration is incomplete</exception>
    /// <exception cref="FileNotFoundException">Directory/file not found</exception>
    public async Task<object> ReadSchema(string fileName)
    {
        var schemasDirectory = _configuration.GetValue<string>("SchemasDirectory") ??
                               throw new InvalidOperationException("SchemasDirectory is null");
        var filePath = Path.Combine(schemasDirectory, fileName);
        if (!Directory.Exists(schemasDirectory) || !File.Exists(filePath))
        {
            throw new FileNotFoundException($"{fileName} not found in {schemasDirectory}");
        }
        var obj = this._parameterSchemaParser.Read(filePath);
        return obj;
    }

    /// <summary>
    ///     Writes the content of a YAML string to a YAML file with form in the schemas directory.
    /// </summary>
    public async Task WriteSchema(string content, string fileName)
    {
        string configDirectory = _configuration.GetValue<string>("SchemasDirectory");
        if (!Directory.Exists(configDirectory))
        {
            Directory.CreateDirectory(configDirectory);
            _logger.LogDebug($"Directory '{configDirectory}' was created");
        }

        string filePath = Path.Combine(configDirectory, fileName);

        await WriteYaml(content, filePath);
    }

    private static async Task WriteYaml(string content, string filePath)
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
