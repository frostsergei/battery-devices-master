using BatteryDevicesMaster.Server.Models;

namespace BatteryDevicesMaster.Server.Services;

public class PresetSchemasReader
{
    public List<ParametersSchema> ReadFiles(string directoryPath)
    {
        var filesData = new List<ParametersSchema>();

        if (Directory.Exists(directoryPath))
        {
            var files = Directory.GetFiles(directoryPath);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var fileData = File.ReadAllText(file);

                filesData.Add(new ParametersSchema
                {
                    FileName = fileName,
                    FileData = fileData
                });
            }
            return filesData;
        }

        return null;
    }
}