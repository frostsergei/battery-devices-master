using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    public class ConditionsValidator : TypeBasedValidator<Dictionary<string, object>>
    {
        public ConditionsValidator() : base(ConditionsKey, KeyType.Additional)
        {
            
        }

        protected override void ValidateImpl(string parameterName, ParameterObjectDict parameters)
        {
            var parameter = parameters[parameterName];
            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.Composite:
                case Type.String:
                case Type.Integer:
                case Type.Decimal: 
                case Type.Date:
                case Type.Time:
                case Type.Selector:
                case Type.Array:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }

            ConditionsValidate(parameterName, parameters);
        }

        private void ConditionsValidate(string parameterName, ParameterObjectDict parameters)
        {
            var conditions = ValidateType(parameterName, parameters);
            if(!conditions.ContainsKey(TypeKey)){
                conditions[TypeKey] = parameters[parameterName][TypeKey];
            }
            parameters[ConditionsKey] = conditions;

            Parameter.Validate(ConditionsKey, parameters);
            parameters.Remove("conditions");
        }
    }
}
