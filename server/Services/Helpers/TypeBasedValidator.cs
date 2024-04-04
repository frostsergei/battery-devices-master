namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public class TypeBasedValidator<T>(string key, KeyType keyType, T? defaultValue = default) : Validator
    {
        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            ValidateType(parameterName, parameters);
        }

        protected T ValidateType(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];

            if (!parameter.TryGetValue(Key, out var valueObj))
            {
                parameter.Add(Key, defaultValue!);
                return defaultValue!;
            }

            if (valueObj is not T value)
            {
                throw new ParameterSchemaParsingException(
                    $"Invalid value for '{Key}' key. It must be a {nameof(T)}",
                    ParameterSchemaLevel.Parameter);
            }

            return value;
        }

        public sealed override string Key => key;
        public sealed override KeyType KeyType => keyType;
    }
}