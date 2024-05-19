using BatteryDevicesMaster.Server.Services;
using BatteryDevicesMaster.Server.Services.Helpers;

namespace BatteryDevicesMaster.Server.Tests;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public class RegexValidatorTest
{
    private static ParameterObject GetParameter(string type, string? regex)
    {
        return new ParameterObject
        {
            { Parameter.NameKey, "regex-test" },
            { Parameter.TypeKey, type },
            { Parameter.RegexKey, regex! },
        };
    }

    public static IEnumerable<object[]> HappyPathData
    {
        get
        {
            yield return new object[] { GetParameter("string", "regex") };
            yield return new object[] { GetParameter("string", "[a-zA-Z]+") };
            yield return new object[] { GetParameter("string", @"^\S+@\S+\.\S+$") };
        }
    }

    [Theory]
    [MemberData(nameof(HappyPathData))]
    public void Validate_HappyPath(ParameterObject parameter)
    {
        Parameter.Validator validator = new Parameter.RegexValidator();
        validator.Validate(parameter, new ParameterObjectDict());
    }

    public static IEnumerable<object[]> IncorrectTypeData
    {
        get
        {
            yield return new object[] { GetParameter("time", "[a-zA-Z]+") };
            yield return new object[] { GetParameter("integer", @"^\S+@\S+\.\S+$") };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectTypeData))]
    public void Validate_IncorrectType(ParameterObject parameter)
    {
        Parameter.Validator validator = new Parameter.RegexValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, new ParameterObjectDict()));
    }

    public static IEnumerable<object[]> IncorrectRegexData
    {
        get
        {
            yield return new object[] { GetParameter("string", "") };
            yield return new object[] { GetParameter("string", "    ") };
            yield return new object[] { GetParameter("string", null) };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectRegexData))]
    public void Validate_IncorrectRegex(ParameterObject parameter)
    {
        Parameter.Validator validator = new Parameter.RegexValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, new ParameterObjectDict()));
    }
}