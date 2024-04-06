using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    public class DependsOnValidator : TypeBasedValidator<string>
    {
        public DependsOnValidator() : base(DependsOnKey, KeyType.Additional)
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
            HasParameterOfDependence(parameterName, parameters);
        }

        private void HasParameterOfDependence(string parameterName, ParameterObjectDict parameters)
        {
            var parametrName = ValidateType(parameterName, parameters);
            if(!parameters.ContainsKey(parametrName)){
                throw new ParameterSchemaParsingException(
                    $"Parameter '{parametrName}' is missing, cannot depend on it",
                     ParameterSchemaLevel.Parameter);
            }
        }
    }
}
