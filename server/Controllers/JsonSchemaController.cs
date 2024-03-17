using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BatteryDevicesMaster.Server.Controllers;

/// <summary>
///     Controller for '/JsonSchema' path
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class JsonSchemaController : ControllerBase
{
    /// <summary>
    ///      JsonSchema handler
    /// </summary>
    /// <response code="200">Dummy json schema</response>
    [HttpGet]
    [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
    public ActionResult<JsonResult> GetJsonSchema()
    {
        var dummySchema = new 
        {
            schema = new 
            {
                title = "A registration form",
                description = "A simple form example.",
                type = "object",
                required = new string[] { "firstName", "lastName" },
                properties = new 
                {
                    firstName = new 
                    {
                        type = "string",
                        title = "First name",
                        @default = "Chuck"
                    },
                    lastName = new 
                    {
                        type = "string",
                        title = "Last name"
                    },
                    age = new 
                    {
                        type = "integer",
                        title = "Age"
                    },
                    bio = new 
                    {
                        type = "string",
                        title = "Bio"
                    },
                    password = new 
                    {
                        type = "string",
                        title = "Password",
                        minLength = 3
                    },
                    telephone = new 
                    {
                        type = "string",
                        title = "Telephone",
                        minLength = 10
                    }
                }
            },
            model = new 
            {
                lastName = "Norris",
                age = 75,
                bio = "Roundhouse kicking asses since 1940",
                password = "noneed"
            }
        };
        return new JsonResult(dummySchema);
    }
}