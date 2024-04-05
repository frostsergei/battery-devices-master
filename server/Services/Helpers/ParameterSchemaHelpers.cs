namespace BatteryDevicesMaster.Server.Services.Helpers;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public static class ParameterSchemaHelpers
{
    public const string TemplatesKey = "templates";
    public const string ParametersKey = "parameters";
}

public static partial class Parameter
{
    public static string GetName(ParameterObject parameter)
    {
        return parameter[NameKey].ToString() ?? throw new ParameterSchemaParsingException(
            $"Parameter name is not a string in the '{ParameterSchemaHelpers.ParametersKey}' section",
            ParameterSchemaLevel.Parameter);
    }

    public static void Validate(string parameterName, ParameterObjectDict parameters)
    {
        var parameter = parameters[parameterName];

        ValidateFieldNames(parameter);
        foreach (var v in Validators)
        {
            v.Validate(parameterName, parameters);
        }
    }

    public static void ValidateFieldNames(ParameterObject parameter)
    {
        foreach (var (key, _) in parameter)
        {
            if (!Keys.Contains(key))
            {
                throw new ParameterSchemaParsingException(
                    $"Invalid key '{key}' in the parameter object",
                    ParameterSchemaLevel.Parameter);
            }
        }
    }

    public static Dictionary<string, Validator> GetValidatorMap()
    {
        return Validators.ToDictionary(v => v.Key);
    }

    public static readonly string[] Keys =
    {
        NameKey, DescriptionKey, NullableKey, EnabledKey, TypeKey, PrecisionKey, MinKey, MaxKey, RegexKey, ItemsKey,
        ConditionsKey, DependsOnKey, OneOfKey, AllOfKey, UsingKey, ForKey
    };

    public static readonly List<Validator> Validators = new()
    {
        new TypeValidator(),
        new TypeBasedValidator<string>(DescriptionKey, KeyType.Additional),
        new TypeBasedValidator<bool>(EnabledKey, KeyType.Optional, defaultValue: true),
        new TypeBasedValidator<bool>(NullableKey, KeyType.Optional, defaultValue: false),
        new PrecisionValidator(),
        new MinMaxValidator(MinMaxType.Min),
        new MinMaxValidator(MinMaxType.Max),
        new RegexValidator()
        // items
        // conditions
        // dependsOn
        // onOf
        // allOf
        // using
        // for
    };

    public const string NameKey = "name";
    public const string DescriptionKey = "description";
    public const string NullableKey = "nullable";
    public const string EnabledKey = "enabled";
    public const string TypeKey = "type";
    public const string PrecisionKey = "precision";
    public const string MinKey = "min";
    public const string MaxKey = "max";
    public const string RegexKey = "regex";
    public const string ItemsKey = "items";
    public const string ConditionsKey = "conditions";
    public const string DependsOnKey = "dependsOn";
    public const string OneOfKey = "oneOf";
    public const string AllOfKey = "allOf";
    public const string UsingKey = "using";
    public const string ForKey = "for";
}
