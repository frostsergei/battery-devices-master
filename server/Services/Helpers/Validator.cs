namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    /// <summary>
    ///     Keys that are used by validators.
    ///     - Required — parameter must always have this key.
    ///     - Optional — parameter may not have this key, validation is enabled anyway.
    ///     - Additional — parameter may not have this key — in this case, validation is disabled.
    ///     - Exceptional — need manual validation, validation is enabled anyway.
    /// </summary>
    public enum KeyType
    {
        Required,
        Optional,
        Additional,
        Exceptional,
    }

    public abstract class Validator
    {
        public void Validate(ParameterObject parameter, ParameterObjectDict parameters)
        {
            if (!parameter.ContainsKey(Key))
            {
                switch (KeyType)
                {
                    case KeyType.Required:
                        throw new ParameterSchemaParsingException(
                            $"Parameter '{Key}' is missing in the 'parameters' section",
                            ParameterSchemaLevel.Parameter);
                    case KeyType.Optional:
                    case KeyType.Exceptional:
                        break;
                    case KeyType.Additional:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown key type '{KeyType.ToString()}'");
                }
            }

            ValidateImpl(parameter, parameters);
        }

        protected abstract void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters);

        public abstract string Key { get; }
        public abstract KeyType KeyType { get; }
    }
}
