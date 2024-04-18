using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BatteryDevicesMaster.Server.Services;

/// <summary>
///     Service for writing Xml db files.
/// </summary>
public class XmlSerializer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<XmlSerializer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlSerializer"/> class.
    /// </summary>
    public XmlSerializer(IConfiguration configuration, ILogger<XmlSerializer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    ///     Writes the content of JSON string to XML db file.
    /// </summary>
    public void WriteXml(string jsonString, string fileName)
    {
        var node = ConvertJsonToXml(jsonString);
        string configDirectory = _configuration.GetValue<string>("XmlDbDirectory");

        if (!Directory.Exists(configDirectory))
        {
            Directory.CreateDirectory(configDirectory);
            _logger.LogDebug($"Directory '{configDirectory}' was created");
        }

        using StreamWriter streamWriter = new StreamWriter(Path.Combine(configDirectory, fileName));
        XmlWriterSettings settings = new XmlWriterSettings { Indent = true, Encoding = System.Text.Encoding.UTF8 };

        using XmlWriter xmlWriter = XmlWriter.Create(streamWriter, settings);
        node?.WriteTo(xmlWriter);
    }

    /// <summary>
    ///     Checks JSON string validation and parses JSON string to XDocument type
    /// </summary>
    private static XDocument? ConvertJsonToXml(string jsonString)
    {
        JToken.Parse(jsonString);
        return JsonConvert.DeserializeXNode(jsonString);
    }
    /// <summary>
    ///     Returns path to file inside XmlDbDirectory 
    /// </summary>
    public string GetPathToXmlFile(string fileName)
    {
        return Path.Combine(_configuration.GetValue<string>("XmlDbDirectory"), fileName);
    }
}
