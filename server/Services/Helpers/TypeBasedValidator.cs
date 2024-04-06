namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public class TypeBasedValidator<T> : Validator
    {
        public TypeBasedValidator(string key, KeyType keyType, T defaultValue = default!)
        {
            Key = key;
            KeyType = keyType;
            _defaultValue = defaultValue;
        }

        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            ValidateType(parameterName, parameters);
        }

        protected T ValidateType(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];

            if (!parameter.TryGetValue(Key, out var valueObj))
            {
                if (_defaultValue != null)
                {
                    parameter.Add(Key, _defaultValue);
                }

                return _defaultValue!;
            }

            if (valueObj is not T value)
            {
                throw new ParameterSchemaParsingException(
                    $"Invalid value for '{Key}' key. It must be a {typeof(T).Name}",
                    ParameterSchemaLevel.Parameter);
            }

            return value;
        }

        public sealed override string Key { get; }
        public sealed override KeyType KeyType { get; }

        private readonly T _defaultValue;
    }
}
