namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public enum KeyType
    {
        Required,
        Optional,
        Additional
    }

    public abstract class Validator
    {
        public void Validate(string parameterName, ParameterObjectDict parameters)
        {
            if (!parameters[parameterName].ContainsKey(Key))
            {
                switch (KeyType)
                {
                    case KeyType.Required:
                        throw new ParameterSchemaParsingException(
                            $"Parameter '{Key}' is missing in the 'parameters' section",
                            ParameterSchemaLevel.Parameter);
                    case KeyType.Optional:
                        break;
                    case KeyType.Additional:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown key type '{KeyType.ToString()}'");
                }
            }

            ValidateImpl(parameterName, parameters);
        }

        protected abstract void ValidateImpl(string parameterName, ParameterObjectDict parameters);

        public abstract string Key { get; }
        public abstract KeyType KeyType { get; }
    }
}