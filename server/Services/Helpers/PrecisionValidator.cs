namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public class PrecisionValidator : TypeBasedValidator<int>
    {
        public PrecisionValidator() : base(PrecisionKey, KeyType.Additional)
        {
        }

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
                        $"Type '{type.ToString()}' must not have 'precision' setting",
                        ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }

            var value = ValidateType(parameterName, parameters);

            if (value < 0)
            {
                throw new ParameterSchemaParsingException(
                    $"Invalid value for '{Key}' Key. Value must not be negative",
                    ParameterSchemaLevel.Parameter);
            }
        }
    }
}
