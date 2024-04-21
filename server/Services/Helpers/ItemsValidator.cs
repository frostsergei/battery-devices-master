namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static partial class Parameter
{
    public const string ItemsKey = "items";
    public const string LabelKey = "label";

    public class ItemsValidator : Validator
    {
        public ItemsValidator()
        {
        }

        protected override void ValidateImpl(ParameterObject parameter, ParameterObjectDict parameters)
        {
            var type = Parameter.GetType(parameter);
            switch (type)
            {
                case Type.Selector:
                    ValidateSelector(parameter[Key]);
                    break;
                case Type.Array:
                    ValidateArray(parameter[Key], parameters);
                    break;
                case Type.String:
                case Type.Integer:
                case Type.Decimal:
                case Type.Date:
                case Type.Time:
                case Type.Composite:
                    throw new ParameterSchemaParsingException(
                        $"Type '{type.ToString()}' must not have 'regex'", ParameterSchemaLevel.Parameter);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type {type.ToString()}");
            }
        }

        private static void ValidateSelector(object items)
        {
            switch (items)
            {
                case List<string>:
                    break;
                case List<Dictionary<string, string>> complexLabels:
                    foreach (var complexLabel in complexLabels)
                    {
                        if (complexLabel.Keys.Count != 2 ||
                            !complexLabel.ContainsKey(LabelKey) || !complexLabel.ContainsKey(DescriptionKey))
                        {
                            throw new ParameterSchemaParsingException("Incorrect selector item",
                                ParameterSchemaLevel.Parameter);
                        }
                    }

                    break;
                default:
                    throw new ParameterSchemaParsingException("Incorrect items type'", ParameterSchemaLevel.Parameter);
            }
        }

        private static void ValidateArray(object value, ParameterObjectDict parameters)
        {
            var parameter = value as ParameterObject ?? throw new ParameterSchemaParsingException(
                $"Items in array must be of type '{typeof(ParameterObject).Name}'", ParameterSchemaLevel.Parameter);
            Parameter.Validate(parameter, parameters);
        }

        public override string Key => ItemsKey;
        public override KeyType KeyType => KeyType.Required;
    }
}
