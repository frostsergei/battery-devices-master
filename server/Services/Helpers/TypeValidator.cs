namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string TypeKey = "type";

    public enum Type
    {
        Integer,
        Decimal,
        String,
        Date,
        Time,
        Selector,
        Array,
        Composite,
    }

    public static Type ParseType(string type)
    {
        return type switch
        {
            "integer" => Type.Integer,
            "decimal" => Type.Decimal,
            "string" => Type.String,
            "date" => Type.Date,
            "time" => Type.Time,
            "selector" => Type.Selector,
            "array" => Type.Array,
            "composite" => Type.Composite,
            _ => throw new ArgumentException($"Unknown type '{type}'"),
        };
    }

    public static Type GetType(ParameterObject parameter)
    {
        return ParseType(parameter[TypeKey].ToString()!);
    }

    public sealed class TypeValidator() : TypeBasedValidator<string>(TypeKey, KeyType.Required)
    {
        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            var value = ValidateType(parameterName, parameters);
            ParseType(value);
        }
    }
}