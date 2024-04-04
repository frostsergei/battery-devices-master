using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    public enum MinMaxType
    {
        Min,
        Max,
    }

    public class MinMaxValidator(MinMaxType minMaxType)
        : TypeBasedValidator<decimal>(GetKey(minMaxType), KeyType.Additional)
    {
        public static string GetKey(MinMaxType minMaxType) => minMaxType == MinMaxType.Max ? MaxKey : MinKey;

        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];

            var type = Parameter.GetType(parameter);

            switch (type)
            {
                case Type.Integer:
                case Type.Decimal:
                    break;
                case Type.String:
                case Type.Date:
                case Type.Time:
                case Type.Selector:
                case Type.Array:
                case Type.Composite:
                    throw new ParameterSchemaParsingException(
                        $"Type '{type.ToString()}' must not have 'min' or 'max' setting",
                        ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown minMaxType {type.ToString()}");
            }

            ValidateType(parameterName, parameters);
        }
    }
}