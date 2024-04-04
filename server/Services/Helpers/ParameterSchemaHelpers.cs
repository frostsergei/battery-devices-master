namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;

public static class ParameterSchemaHelpers
{
    public const string TemplatesKey = "templates";
    public const string ParametersKey = "parameters";
}

public static partial class Parameter
{
    public const string NameKey = "name";
    public const string DescriptionKey = "description";
    public const string NullableKey = "nullable";
    public const string EnabledKey = "enabled";

    public const string RegexKey = "regex";
    public const string ItemsKey = "items";
    public const string ConditionsKey = "conditions";
    public const string DependsOnKey = "dependsOn";
    public const string OneOfKey = "oneOf";
    public const string AllOfKey = "allOf";
    public const string UsingKey = "using";
    public const string ForKey = "for";

    public static string GetName(ParameterObject parameter)
    {
        return parameter[NameKey].ToString() ?? throw new ParameterSchemaParsingException(
            $"Parameter name is not a string in the '{ParameterSchemaHelpers.ParametersKey}' section",
            ParameterSchemaLevel.Parameter);
    }

    public static readonly Dictionary<string, Validator> FieldValidators =
        new()
        {
            { TypeKey, new TypeValidator() },
            { DescriptionKey, new TypeBasedValidator<string>(DescriptionKey, KeyType.Additional) },
            { EnabledKey, new TypeBasedValidator<bool>(EnabledKey, KeyType.Optional, defaultValue: true) },
            { NullableKey, new TypeBasedValidator<bool>(NullableKey, KeyType.Optional, defaultValue: false) },
            { PrecisionKey, new PrecisionValidator() },
            { MinKey, new MinMaxValidator(MinMaxType.Min) },
            { MaxKey, new MinMaxValidator(MinMaxType.Max) },
            // { RegexKey, KeyType.Additional },
            // { ItemsKey, KeyType.Additional },
            // { ConditionsKey, KeyType.Additional },
            // { DependsOnKey, KeyType.Additional },
            // { OneOfKey, KeyType.Additional },
            // { AllOfKey, KeyType.Additional },
            // { UsingKey, KeyType.Additional },
            // { ForKey, KeyType.Additional },
        };
}