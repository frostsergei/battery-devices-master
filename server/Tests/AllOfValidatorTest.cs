using BatteryDevicesMaster.Server.Services;
using BatteryDevicesMaster.Server.Services.Helpers;
using Xunit;

namespace BatteryDevicesMaster.Server.Tests;

using ParameterObject = Dictionary<string, object>;
using ParameterObjectDict = Dictionary<string, Dictionary<string, object>>;

public class AllOfValidatorTest
{
    private static ParameterObject GetParameter(string name, string type, List<string> allOf)
    {
        return new ParameterObject
        {
            { Parameter.NameKey, name },
            { Parameter.TypeKey, type },
            { Parameter.AllOfKey, allOf },
        };
    }

    private static ParameterObject GetParameter(string name, string type, string? allOf)
    {
        return new ParameterObject
        {
            { Parameter.NameKey, name },
            { Parameter.TypeKey, type },
            { Parameter.AllOfKey, allOf! },
        };
    }

    private static ParameterObject GetParameter(string name, string type)
    {
        return new ParameterObject
        {
            { Parameter.NameKey, name },
            { Parameter.TypeKey, type },
        };
    }

    public static IEnumerable<object[]> HappyPathData
    {
        get
        {
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", new List<string> { "child-1", "child-2", "child-3" }),
                new ParameterObjectDict
                {
                    { "child-1", GetParameter("child-1", "integer") },
                    { "child-2", GetParameter("child-2", "string") },
                    { "child-3", GetParameter("child-3", "selector") },
                }
            };
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", new List<string> { "child" }),
                new ParameterObjectDict
                {
                    { "child", GetParameter("child", "array") },
                }
            };
        }
    }

    [Theory]
    [MemberData(nameof(HappyPathData))]
    public void Validate_HappyPath(ParameterObject parameter, ParameterObjectDict parameters)
    {
        Parameter.Validator validator = new Parameter.AllOfValidator();
        validator.Validate(parameter, parameters);
    }

    public static IEnumerable<object[]> IncorrectTypeData
    {
        get
        {
            yield return new object[]
            {
                GetParameter("allOf-parent", "integer", new List<string> { "child" }),
                new ParameterObjectDict
                {
                    { "child", GetParameter("child", "array") },
                }
            };
            yield return new object[]
            {
                GetParameter("allOf-parent", "array", new List<string> { "child" }),
                new ParameterObjectDict
                {
                    { "child", GetParameter("child", "array") },
                }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectTypeData))]
    public void Validate_IncorrectType(ParameterObject parameter, ParameterObjectDict parameters)
    {
        Parameter.Validator validator = new Parameter.AllOfValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, parameters));
    }

    public static IEnumerable<object[]> IncorrectAllOfData
    {
        get
        {
            // Empty 'allOf' list
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", new List<string>()),
                new ParameterObjectDict
                {
                    { "child", GetParameter("child", "array") },
                }
            };
            // Null 'allOf' value
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", (string)null!),
                new ParameterObjectDict
                {
                    { "child", GetParameter("child", "array") },
                }
            };
            // String 'allOf' value
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", "[\"child\"]"),
                new ParameterObjectDict
                {
                    { "child", GetParameter("child", "array") },
                }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectAllOfData))]
    public void Validate_IncorrectAllOf(ParameterObject parameter, ParameterObjectDict parameters)
    {
        Parameter.Validator validator = new Parameter.AllOfValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, parameters));
    }

    public static IEnumerable<object[]> IncorrectAllOfChildrenData
    {
        get
        {
            // Child does not exist
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", new List<string> { "child-1", "child-ERROR", "child-3" }),
                new ParameterObjectDict
                {
                    { "child-1", GetParameter("child-1", "string") },
                    { "child-2", GetParameter("child-2", "integer") },
                    { "child-3", GetParameter("child-3", "selector") },
                }
            };
            // composite child
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", new List<string> { "child-1", "child-2", "child-3" }),
                new ParameterObjectDict
                {
                    { "child-1", GetParameter("child-1", "string") },
                    { "child-2", GetParameter("child-2", "composite") },
                    { "child-3", GetParameter("child-3", "selector") },
                }
            };
            // Self in children list
            yield return new object[]
            {
                GetParameter("allOf-parent", "composite", new List<string> { "child-1", "allOf-parent", "child-3" }),
                new ParameterObjectDict
                {
                    { "child-1", GetParameter("child-1", "string") },
                    { "child-2", GetParameter("child-2", "integer") },
                    { "child-3", GetParameter("child-3", "selector") },
                }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IncorrectAllOfChildrenData))]
    public void Validate_IncorrectAllOfChildren(ParameterObject parameter, ParameterObjectDict parameters)
    {
        Parameter.Validator validator = new Parameter.AllOfValidator();
        Assert.Throws<ParameterSchemaParsingException>(() => validator.Validate(parameter, parameters));
    }
}