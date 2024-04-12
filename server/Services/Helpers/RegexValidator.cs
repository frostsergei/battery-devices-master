using System.Text.RegularExpressions;

namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string RegexKey = "regex";

    public class RegexValidator : TypeBasedValidator<string>
    {
        public RegexValidator() : base(RegexKey, KeyType.Additional)
        {
        }

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.String:
                    break;
                case Type.Integer:
                case Type.Decimal:
                case Type.Date:
                case Type.Time:
                case Type.Selector:
                case Type.Array:
                case Type.Composite:
                    throw new ParameterSchemaParsingException(
                        $"Type '{type.ToString()}' must not have 'regex'", ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }

            string value = ValidateType(parameter, parameters);
            ValidateRegexPattern(value);
        }

        private static void ValidateRegexPattern(string pattern)
        {
            try
            {
                _ = Regex.Match(string.Empty, pattern);
            }
            catch (ArgumentException)
            {
                throw new ParameterSchemaParsingException(
                    $"'{pattern}' is not a valid regex.", ParameterSchemaLevel.Parameter);
            }
        }
    }
}
