using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDictionary = Dictionary<string, Dictionary<string, object>>;

public partial class Parameter
{
    public const string UsingKey = "using";

    private static ParameterObjectDictionary GetTemplateMap(List<ParameterObject> templates)
    {
        ParameterObjectDictionary templateMap = new();

        foreach (var template in templates)
        {
            if (!template.Remove(NameKey, out var nameObject))
            {
                throw new ParameterSchemaParsingException("template does not have 'name' key",
                    ParameterSchemaLevel.Templates);
            }

            var name = nameObject as string ??
                       throw new ParameterSchemaParsingException("name is not a string",
                           ParameterSchemaLevel.Templates);
            templateMap[name] = template;
        }

        return templateMap;
    }

    private static string ReplaceTemplateArgs(string value, string[] args)
    {
        Regex regex = new(@"\$(\d+)");
        var matches = regex.Matches(value);

        foreach (Match match in matches)
        {
            var groups = match.Groups;
            var index = int.Parse(groups[1].Value);

            if (args.Length <= index)
            {
                throw new ParameterSchemaParsingException("not enough args for a template",
                    ParameterSchemaLevel.Templates);
            }

            value = Regex.Replace(value, $@"\${index}", args[index]);
        }

        return value;
    }

    private static void FillParameterWithTemplate(ParameterObject parameter, ParameterObjectDictionary templateMap)
    {
        const string separator = ":";

        if (!parameter.Remove(UsingKey, out var valueObj))
        {
            return;
        }

        var value = valueObj as string ??
                    throw new ParameterSchemaParsingException("using value is not a string",
                        ParameterSchemaLevel.Templates);
        var templateArgs = value.Split(separator);
        var templateName = templateArgs[0];

        if (!templateMap.TryGetValue(templateName, out var template))
        {
            throw new ParameterSchemaParsingException($"template {templateName} does not exist",
                ParameterSchemaLevel.Templates);
        }

        foreach (var (key, tValue) in template)
        {
            if (parameter.ContainsKey(key))
            {
                continue;
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            
            var tValueStr = serializer.Serialize(tValue) ?? 
                            throw new ParameterSchemaParsingException("cannot convert tValue to string", 
                                ParameterSchemaLevel.Templates);
            
            // var tValueStr = tValue as string ??
            //                 throw new ParameterSchemaParsingException("cannot convert tValue to string",
            //                     ParameterSchemaLevel.Templates);
            parameter[key] = ReplaceTemplateArgs(tValueStr, templateArgs);
        }
    }

    public static void OpenTemplates(List<ParameterObject> parameters, List<ParameterObject> templates)
    {
        var templateMap = GetTemplateMap(templates);

        foreach (var parameter in parameters)
        {
            FillParameterWithTemplate(parameter, templateMap);
        }
    }
}