using System.ComponentModel.DataAnnotations;

namespace BatteryDevicesMaster.Server.Models;

/// <summary>
///     Error response with message
/// </summary>
public struct ErrorResponse
{
    /// <summary>
    ///     Error message
    /// </summary>
    [Required]
    public string Message { get; set; }
}