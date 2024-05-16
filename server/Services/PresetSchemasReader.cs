using BatteryDevicesMaster.Server.Models;

namespace BatteryDevicesMaster.Server.Services;

public class PresetSchemasReader
{
    public List<ParametersSchema>? ReadFiles(string directoryPath, string parameterFilename, string formFilename)
    {
        var filesData = new List<ParametersSchema>();

        foreach (var dir in Directory.GetDirectories(directoryPath))
        {
            if (Directory.Exists(dir))
            {
                var parameterFilePath = Path.Combine(dir, parameterFilename);
                var formFilePath = Path.Combine(dir, formFilename);
                
                if (!System.IO.File.Exists(parameterFilePath) || !System.IO.File.Exists(formFilePath))
                {
                    throw new InvalidOperationException("One or both YAML files not found");
                }
                
                var parameterYaml = System.IO.File.ReadAllText(parameterFilePath);
                var formYaml = System.IO.File.ReadAllText(formFilePath);
                
                var combinedYaml = parameterYaml + "\n" + formYaml;
                
                filesData.Add(new ParametersSchema
                {
                    FileName = new DirectoryInfo(dir).Name,
                    FileData = combinedYaml
                });
            }
        }
    
        return filesData.Count > 0 ? filesData : null;
    }
}