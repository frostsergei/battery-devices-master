using System.Text.RegularExpressions;
using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    public class RegexValidator : TypeBasedValidator<string>
    {
        public RegexValidator() : base(RegexKey, KeyType.Optional)
        {
            
        }

        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];

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
                        $"Type '{type.ToString()}' must not have 'regex'",
                        ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }
            
            string value = ValidateType(parameterName, parameters);
            ValidateRegexPattern(value);
        }
        private void ValidateRegexPattern(string pattern)
        {
            try
            {
                Regex.Match(string.Empty, pattern); 
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"String \"{pattern}\" is not a valid regex.");
            }
        }
    }
}
