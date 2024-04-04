using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BatteryDevicesMaster.Server.Services;

/// <summary>
///     Service for writing Xml db files.
/// </summary>
public class XmlWriteService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SchemaSerializer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlWriteService"/> class.
    /// </summary>
    public XmlWriteService(IConfiguration configuration, ILogger<SchemaSerializer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    ///     Writes the content of JSON string to XML db file.
    /// </summary>
    public void WriteXml(string jsonString, string fileName)
    {
        XNode node = ParseJsonToXml(jsonString);
        string configDirectory = _configuration.GetValue<string>("XmlDbDirectory");

        if (!Directory.Exists(configDirectory))
        {
            Directory.CreateDirectory(configDirectory);
            _logger.LogDebug($"Directory '{configDirectory}' was created");
        }

        using (StreamWriter streamWriter = new StreamWriter(Path.Combine(configDirectory, fileName)))
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, settings))
            {
                node.WriteTo(xmlWriter);
            }
        }
    }

    /// <summary>
    ///     Checks JSON string validation and parses JSON string to XDocument type
    /// </summary>
    private XDocument? ParseJsonToXml(string jsonString)
    {
        JToken.Parse(jsonString);
        return JsonConvert.DeserializeXNode(jsonString);
    }
}
