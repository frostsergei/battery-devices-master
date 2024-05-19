namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string PrecisionKey = "precision";

    public class PrecisionValidator : TypeBasedValidator<int>
    {
        public PrecisionValidator() : base(PrecisionKey, KeyType.Additional)
        {
        }

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.Decimal:
                    break;
                case Type.Integer:
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

            var value = ValidateType(parameter);

            if (value < 0)
            {
                throw new ParameterSchemaParsingException(
                    $"Invalid value for '{Key}' Key. Value must not be negative",
                    ParameterSchemaLevel.Parameter);
            }
        }
    }
}
