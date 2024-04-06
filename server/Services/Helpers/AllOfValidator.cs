using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    public class AllOfValidator : TypeBasedValidator<string[]>
    {
        public AllOfValidator() : base(AllOfKey, KeyType.Additional)
        {
            
        }

        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];
            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.Composite:
                    break;
                case Type.String:
                case Type.Integer:
                case Type.Decimal: 
                case Type.Date:
                case Type.Time:
                case Type.Selector:
                case Type.Array:
                    throw new ParameterSchemaParsingException(
                        $"Type '{type.ToString()}' must not have 'allOf'",
                        ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }
            ValidateAgainstCompositeParametersInAllOf(parameterName, parameters);
        }

        private void ValidateAgainstCompositeParametersInAllOf(string parameterName, ParameterObjectDict parameters)
        {
            var atomicParametersNames = ValidateType(parameterName, parameters);
            foreach (var atomicParameterName in atomicParametersNames)
            {
                var atomicParameter = parameters[atomicParameterName];
                Type atomicParameterType = Parameter.GetType(atomicParameter);
                if (atomicParameterType == Type.Composite)
                {
                    throw new ArgumentException($"Composite parameter '{parameterName}' can't contain other composite parameter '{atomicParameterName}'");
                }
            }
        }
    }
}
