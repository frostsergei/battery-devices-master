using BatteryDevicesMaster.Server.Services;
using BatteryDevicesMaster.Server.Services.Helpers;
using Xunit;

namespace BatteryDevicesMaster.Server.Tests;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public class PrecisionValidatorTest
{
    private static ParameterObject GetParameter(string type, int precision)
    {
        return new ParameterObject
        {
            { Parameter.NameKey, "precision-test" },
            { Parameter.TypeKey, type },
            { Parameter.PrecisionKey, precision },
        };
    }
    
    private static ParameterObject GetParameter(string type, double? precision)
    {
        return new ParameterObject
        {
            { Parameter.NameKey, "precision-test" },
            { Parameter.TypeKey, type },
            { Parameter.PrecisionKey, precision! },
        };
    }

    public static IEnumerable<object[]> HappyPathData
    {
        get
        {
            yield return new object[] { GetParameter("decimal", 10) };
            yield return new object[] { GetParameter("decimal", 1) };
            yield return new object[] { GetParameter("decimal", 0) };
        }
    }

    [Theory]
    [MemberData(nameof(HappyPathData))]
    public void Validate_HappyPath(ParameterObject parameter)
    {
        Parameter.Validator validator = new Parameter.PrecisionValidator();
        validator.Validate(parameter, new ParameterObjectDict());
    }

    public static IEnumerable<object[]> IncorrectTypeData
    {
        get
        {
            yield return new object[] { GetParameter("integer", 2) };
            yield return new object[] { GetParameter("string", 2) };
            yield return new object[] { GetParameter("date", 2) };
            yield return new object[] { GetParameter("time", 2) };
            yield return new object[] { GetParameter("selector", 2) };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectTypeData))]
    public void Validate_IncorrectType(ParameterObject parameter)
    {
        Parameter.Validator validator = new Parameter.PrecisionValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, new ParameterObjectDict()));
    }

    public static IEnumerable<object[]> IncorrectPrecisionData
    {
        get
        {
            yield return new object[] { GetParameter("decimal", -10) };
            yield return new object[] { GetParameter("decimal", -0.1) };
            yield return new object[] { GetParameter("decimal", null) };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectPrecisionData))]
    public void Validate_IncorrectRegexData(ParameterObject parameter)
    {
        Parameter.Validator validator = new Parameter.PrecisionValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, new ParameterObjectDict()));
    }
}
