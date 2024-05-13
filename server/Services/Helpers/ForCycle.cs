using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;

public static partial class Parameter
{
    // TODO(go1vs1noob): this algo of opening "for" cycles is not in-place yet. do we need to fix it?
    // TODO(go1vs1noob): we have issues if field in parameter looks like this: "min: {i}". {i} after casting is a dictionary {i: } and not a string "{i}"
    // TODO(go1vs1noob): add support for arithmetic operations with 'i'
    public static void OpenForCycles(List<ParameterObject> parameters)
    {
        const string forKey = "for";
        const string forSeparator = ":";
        const string indexerVariable = "{i}";

        var openedParametersToInclude = new List<ParameterObject>();
        var indexedParametersToRemove = new List<ParameterObject>();
        foreach (var parameter in parameters)
        {
            if (!parameter.ContainsKey(forKey))
            {
                continue;
            }

            ProcessParameterContainingForCycle(parameter);
        }

        parameters.AddRange(openedParametersToInclude);
        foreach (var indexedParameter in indexedParametersToRemove)
        {
            parameters.Remove(indexedParameter);
        }

        return;

        void ProcessParameterContainingForCycle(ParameterObject schemaParameter)
        {
            indexedParametersToRemove.Add(schemaParameter);

            var forCycleParts = (schemaParameter[forKey] as string
                                 ?? throw new ParameterSchemaParsingException(
                                     "'for' key must contain value of type 'string'",
                                     ParameterSchemaLevel.Parameter))
                .Split(forSeparator);
            var start = int.Parse(forCycleParts[0]);
            var stop = int.Parse(forCycleParts[1]);
            var step = int.Parse(forCycleParts[2]);

            for (var i = start; i < stop + 1; i += step)
            {
                var openedParameter = new ParameterObject();
                foreach (var (key, value) in schemaParameter)
                {
                    var serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();

                    var valueString = serializer.Serialize(value) ??
                                      throw new ParameterSchemaParsingException("Cannot convert value to string",
                                          ParameterSchemaLevel.Parameter);

                    openedParameter.Add(key,
                        valueString.Contains(indexerVariable)
                            ? valueString.Replace(indexerVariable, i.ToString())
                            : value);
                }

                openedParametersToInclude.Add(openedParameter);
            }
        }
    }
}