using ParameterObjectDict =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;

namespace BatteryDevicesMaster.Server.Services.Helpers;

public static partial class Parameter
{
    public class OneOfValidator : TypeBasedValidator<int[]>
    {
        public OneOfValidator() : base(OneOfKey, KeyType.Additional)
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

            ValidateType(parameterName, parameters);
        }
    }
}
