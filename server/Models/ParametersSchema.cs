namespace BatteryDevicesMaster.Server.Models;

/// <summary>
///     Schemas request body
/// </summary>
public struct ParametersSchema
{
    /// <summary>
    ///     JSON with schemas name and data
    /// </summary>
    public string FileName { get; set; }
    public string FileData { get; set; }
}