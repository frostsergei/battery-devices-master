namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string ConditionsKey = "conditions";

    public class ConditionsValidator : TypeBasedValidator<List<object>>
    {
        public ConditionsValidator() : base(ConditionsKey, KeyType.Additional)
        {
        }

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
            ValidateType(parameter, parameters);
            // TODO(purposelessness): validate conditions
        }
    }
}
