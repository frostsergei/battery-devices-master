namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
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

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
            ValidateType(parameter, parameters);
        }

        protected T ValidateType(ParameterObject parameter, ParameterObjectDict parameters)
        {
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
