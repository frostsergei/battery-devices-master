using System.Collections.Generic;
using System.IO;

namespace BatteryDevicesMaster.Server.Services;

public class PresetSchemasReader
{
    public Dictionary<string, string> ReadFiles(string directoryPath)
    {
        var filesData = new Dictionary<string, string>();

        if (Directory.Exists(directoryPath))
        {
            var files = Directory.GetFiles(directoryPath);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var fileData = File.ReadAllText(file);
                filesData.Add(fileName, fileData);
            }
            return filesData;
        }

        return null;
    }
}