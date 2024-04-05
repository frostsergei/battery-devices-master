using System.Text.RegularExpressions;
using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    
    public class ForValidator : TypeBasedValidator<string>
    {
        private const string ForLoopIndexerString = "i";
        private const string ForLoopDelimiter = ":";
        private const int ForLoopSectionAmount = 3;
        private const int ForLoopStartIndex = 0;
        private const int ForLoopEndIndex = 1;
        
        public ForValidator() : base(ForKey, KeyType.Additional)
        {
            
        }

        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];
            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.String:
                case Type.Integer:
                case Type.Decimal: 
                case Type.Date:
                case Type.Time:
                case Type.Selector:
                case Type.Array:
                    break;
                case Type.Composite:
                    throw new ParameterSchemaParsingException(
                        $"Type '{type.ToString()}' must not have 'for'",
                        ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }
            string value = ValidateType(parameterName, parameters);
            ValidateParameterName(parameter[NameKey] as string ?? throw new ArgumentException("'name' value must be a string"));
            ValidateSyntax(value);
        }
        
        private void ValidateParameterName(string parameterName)
        {
            if (!parameterName.Contains(ForLoopIndexerString))
            {
                throw new ArgumentException($"Provided parameter name '{parameterName}' must contain an indexer '{ForLoopIndexerString}'");
            }
        }

        private void ValidateSyntax(string forString)
        {
            
            string[] startStopStep = forString.Split(ForLoopDelimiter);

            if (startStopStep.Length != ForLoopSectionAmount)
            {
                throw new ArgumentException($"Provided string '{forString}' is not a valid 'for' loop");
            }
            int start = ParseIfValidInteger(startStopStep[ForLoopStartIndex]);
            int end = ParseIfValidInteger(startStopStep[ForLoopEndIndex]);
            if (start > end)
            {
                throw new ArgumentException($"Provided START value '{start}' is greater than provided END value '{end}'");
            }
        }

        private int ParseIfValidInteger(string value)
        {
            if (!int.TryParse(value, out int parsedValue))
            {
                throw new Exception($"Provided value in for loop '{value}' is not a valid integer");
            }
            return parsedValue;
        }
    }
}
