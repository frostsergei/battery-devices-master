using System.ComponentModel.DataAnnotations;

namespace BatteryDevicesMaster.Server.Models;

/// <summary>
///     Text message request/response body
/// </summary>
public struct TextMessage
{
    /// <summary>
    ///     Text message
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Message must not be empty")]
    public string Message { get; set; }
}