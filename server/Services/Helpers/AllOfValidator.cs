namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;
using ReadonlyParameterObjectDict = IReadOnlyDictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string AllOfKey = "allOf";

    public class AllOfValidator : TypeBasedValidator<List<string>>
    {
        public AllOfValidator() : base(AllOfKey, KeyType.Exceptional)
        {
        }

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
            var hasAllOfKey = parameter.ContainsKey(AllOfKey);

            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.Composite:
                    if (!hasAllOfKey)
                    {
                        throw new ParameterSchemaParsingException(
                            $"Type 'complex' must always have 'allOf'", ParameterSchemaLevel.Parameter);
                    }

                    break;
                case Type.String:
                case Type.Integer:
                case Type.Decimal:
                case Type.Date:
                case Type.Time:
                case Type.Selector:
                case Type.Array:
                    if (hasAllOfKey)
                    {
                        throw new ParameterSchemaParsingException(
                            $"Type '{type.ToString()}' must not have 'allOf'", ParameterSchemaLevel.Parameter);
                    }

                    return;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }

            var atomicParameters = ValidateType(parameter);

            ValidateAtomicParameters(atomicParameters, parameters);
        }

        private static void ValidateAtomicParameters(
            List<string>? atomicParameters,
            ReadonlyParameterObjectDict parameters)
        {
            if (atomicParameters == null || atomicParameters.Count == 0)
            {
                throw new ParameterSchemaParsingException(
                    "'allOf' list is null or empty", ParameterSchemaLevel.Parameter);
            }

            foreach (var name in atomicParameters)
            {
                if (!parameters.TryGetValue(name, out var parameter))
                {
                    throw new ParameterSchemaParsingException(
                        $"Atomic parameter '{name}' not found", ParameterSchemaLevel.Parameter);
                }

                var type = Parameter.GetType(parameter);
                if (type == Type.Composite)
                {
                    throw new ParameterSchemaParsingException(
                        $"Composite parameter '{name}' cannot be used in 'allOf' setting",
                        ParameterSchemaLevel.Parameter);
                }
            }
        }
    }
}