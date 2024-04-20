using System.Collections.Specialized;
using BatteryDevicesMaster.Server.Services.Helpers;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace BatteryDevicesMaster.Server.Services;

using ParameterObject = Dictionary<object, object>;
using ParameterObjectDictionary = Dictionary<string, Dictionary<string, object>>;

public enum ParameterSchemaLevel
{
    Base,
    Parameters,
    Parameter,
    Templates,
}

/// <summary>
///     Exception thrown when a parameter schema file cannot be parsed.
/// </summary>
public class ParameterSchemaParsingException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ParameterSchemaParsingException"/> class.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="level">Level of exception occured</param>
    public ParameterSchemaParsingException(string message, ParameterSchemaLevel level) : base(message)
    {
        Level = level;
    }

    private ParameterSchemaLevel Level { get; }
}

/// <summary>
///     A service for serializing and deserializing parameter schema files.
/// </summary>
public class ParameterSchemaParser
{
    /// <summary>
    ///   Reads and validates the content of a parameter schema file.
    /// </summary>
    /// <param name="filePath">YAML file path</param>
    /// <returns>Parameter schema</returns>
    /// <exception cref="ParameterSchemaParsingException">
    ///     Exception thrown when a parameter schema file cannot be parsed
    /// </exception>
    public object Read(string filePath)
    {
        var builder = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = builder.Deserialize(new StringReader(File.ReadAllText(filePath))) ??
                         throw new ParameterSchemaParsingException("YAML file is empty", ParameterSchemaLevel.Base);


        ParameterSchemaValidator.TopLevel(yamlObject, out var parameters, out var templates);
        // TODO(go1vs1noob): call method to open 'templates' here
        OpenForCycles(parameters);

        return yamlObject;
    }

    // TODO(go1vs1noob): this algo of opening "for" cycles is not in-place yet. do we need to fix it?
    // TODO(go1vs1noob): we have issues if field in parameter looks like this: "min: {i}". {i} after casting is a dictionary {i: } and not a string "{i}"
    // TODO(go1vs1noob): add support for arithmetic operations with 'i'
    // TODO(go1vs1noob): add exceptions
    private void OpenForCycles(List<object> parameters)
    {
        const string ForKey = "for";
        const string ForSeparator = ":";
        const string IndexerVariable = "{i}";
        
        var openedParametersToInclude = new List<ParameterObject>();
        var indexedParametersToRemove = new List<ParameterObject>();
        foreach (var parameter in parameters)
        {
            var schemaParameter = parameter as ParameterObject;
            if (!schemaParameter.ContainsKey(ForKey))
            {
                continue;
            }
            ProcessParameterContainingForCycle(schemaParameter);
        }
        SwapIndexedParametersToOpenedParameters();


        void ProcessParameterContainingForCycle(ParameterObject schemaParameter)
        {
            indexedParametersToRemove.Add(schemaParameter);

            string[] forCycleParts = (schemaParameter[ForKey] as string).Split(ForSeparator);
            int start = int.Parse(forCycleParts[0]);
            int stop = int.Parse(forCycleParts[1]);
            int step = int.Parse(forCycleParts[2]);

            for (int i = start; i < stop + 1; i += step)
            {
                var openedParameter = new ParameterObject();
                foreach (var (key, value) in schemaParameter)
                {
                    if (value.ToString().Contains(IndexerVariable))
                    {
                        openedParameter.Add(key, value.ToString().Replace(IndexerVariable, i.ToString()));
                    }
                    else
                    {
                        openedParameter.Add(key, value);
                    }
                }
                openedParametersToInclude.Add(openedParameter);
            }
        }
        void SwapIndexedParametersToOpenedParameters()
        {
            parameters.AddRange(openedParametersToInclude);
            foreach (var indexedParameter in indexedParametersToRemove)
            {
                parameters.Remove(indexedParameter);
            }
        }
    }

}

public static class ParameterSchemaValidator
{
    public static void TopLevel(object yamlObject, out List<object> parameters, out List<object> templates)
    {
        // TODO(go1vs1noob): this throws an exception if we try to parse to Dictionary<string, object>. Is this related to issue 74? 
        var yamlDict = yamlObject as Dictionary<object, object> ??
                       throw new ParameterSchemaParsingException("YAML file is not a dictionary",
                           ParameterSchemaLevel.Base);

        switch (yamlDict.Keys.Count)
        {
            case 0: throw new ParameterSchemaParsingException("Empty YAML file", ParameterSchemaLevel.Base);
            case 1 when !yamlDict.ContainsKey(ParameterSchemaHelpers.ParametersKey):
                throw new ParameterSchemaParsingException(
                    "Invalid key in the YAML file. Only 'parameters' and 'templates' keys are allowed",
                    ParameterSchemaLevel.Base);
            case 2 when !yamlDict.ContainsKey(ParameterSchemaHelpers.TemplatesKey) ||
                        !yamlDict.ContainsKey(ParameterSchemaHelpers.ParametersKey):
                throw new ParameterSchemaParsingException(
                    "Invalid keys in the YAML file. Only 'parameters' and 'templates' keys are allowed",
                    ParameterSchemaLevel.Base);
            case > 2:
                throw new ParameterSchemaParsingException(
                    "Too many keys in the YAML file. Only 'parameters' and 'templates' keys are allowed",
                    ParameterSchemaLevel.Base);
        }

        parameters = yamlDict[ParameterSchemaHelpers.ParametersKey] as List<object> ??
                     throw new ParameterSchemaParsingException(
                         "Invalid 'parameters' section in the YAML file. It must be a list",
                         ParameterSchemaLevel.Parameters);
        templates = new List<object>();

        if (yamlDict.TryGetValue(ParameterSchemaHelpers.TemplatesKey, out var templateList))
        {
            templates = templateList as List<object> ??
                        throw new ParameterSchemaParsingException(
                            "Invalid 'templates' section in the YAML file. It must be a list",
                            ParameterSchemaLevel.Templates);
        }
    }

    public static void Templates(List<object> templatesList, out ParameterObjectDictionary templates)
    {
        templates = new ParameterObjectDictionary();
    }

    public static void Parameters(List<object> parametersList, out ParameterObjectDictionary parameters)
    {
        parameters = new ParameterObjectDictionary();

        foreach (var parameterObj in parametersList)
        {
            var parameter = parameterObj as Dictionary<string, object> ??
                            throw new ParameterSchemaParsingException(
                                "Invalid parameter in the 'parameters' section. It must be a dictionary",
                                ParameterSchemaLevel.Parameter);

            _ = Parameter.GetName(parameter);
            Parameter.Validate(parameter, parameters);
        }
    }
}
