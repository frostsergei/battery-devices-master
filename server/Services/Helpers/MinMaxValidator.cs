namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string MinKey = "min";
    public const string MaxKey = "max";

    public enum MinMaxType
    {
        Min,
        Max,
    }

    public class MinMaxValidator : TypeBasedValidator<decimal>
    {
        public MinMaxValidator(MinMaxType minMaxType) : base(GetKey(minMaxType), KeyType.Additional)
        {
        }

        public static string GetKey(MinMaxType minMaxType) => minMaxType == MinMaxType.Max ? MaxKey : MinKey;

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
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

            ValidateType(parameter);
        }
    }
}
