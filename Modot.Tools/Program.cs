
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Data;
using MiniExcelLibs;
using Modot.Persistence;

namespace Modot.Tools;
internal class Program
{
    private static string CurrentDir = Environment.CurrentDirectory;
    static async Task<int> Main(string[] args)
    {
        var inputPathOption = new Option<string>(
            name: "--input-path", 
            description: "input file path which need convert.");

        var outputPathOption = new Option<string>(
            name: "--output-path", 
            description: "output file path which need convert.");

        var sheetNameOption = new Option<string>(
            name: "--sheet-name", 
            description: "excel sheet name.",
            isDefault: true,
            parseArgument: result =>{
                if (result.Tokens.Count == 0)
                {
                    return string.Empty;
                }
                return result.Tokens.Single().Value;
            });

        var outputDirectoryOption = new Option<string>(
            name: "--output-dir", 
            description: "output file path which need convert.",
            isDefault: true,
            parseArgument: result =>{
                if (result.Tokens.Count == 0)
                {
                    return string.Empty;
                }
                string? directory = result.Tokens.Single().Value;
                if(!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                return directory;
            });

        var outputTypeOption = new Option<string>(
            name: "--output-type", 
            description: "output file type.",
            isDefault: true,
            parseArgument: result =>{
                if (result.Tokens.Count == 0)
                {
                    return string.Empty;
                }
                return result.Tokens.Single().Value;
            }); 

        var convertCommand = new Command("convert", "convert different data format file."){
            inputPathOption,
            outputPathOption,
            sheetNameOption
        };
        var convertAllCommand = new Command("convert-all", "convert all excel data file."){
            inputPathOption,
            outputDirectoryOption,
            outputTypeOption
        };
        var rootCommand = new RootCommand("Modot tool for System.CommandLine");
        rootCommand.AddCommand(convertCommand);
        rootCommand.AddCommand(convertAllCommand);

        convertCommand.SetHandler(ConvertFile, inputPathOption, outputPathOption, sheetNameOption);
        convertAllCommand.SetHandler(ConvertAllFile, inputPathOption, outputDirectoryOption, outputTypeOption);
        
        return await rootCommand.InvokeAsync(args);
    }

    internal static void ConvertFile(string inputPath, string outputPath, string sheetName){
        outputPath = $"{CurrentDir}\\{outputPath}";
        inputPath = $"{CurrentDir}\\{inputPath}";
        List<Dictionary<string, object>> data;
        if (inputPath.EndsWith(".xlsx"))
        {
            data = Excel.FromExcelFile(inputPath, sheetName, ExcelType.XLSX);
        }
        else if (inputPath.EndsWith(".csv"))
        {
            data = Excel.FromExcelFile(inputPath, sheetName, ExcelType.CSV);
        }
        else if (inputPath.EndsWith(".json"))
        {
            data = Json.FromJsonFile(inputPath);
        }
        else if (inputPath.EndsWith(".yaml"))
        {
            data = Yaml.FromYamlFile(inputPath);
        }
        else
        {
            throw new Exception("dont provide this input file format");
        }

        if (outputPath.EndsWith(".xlsx")){
            Excel.ToExcelFile<object>(outputPath, sheetName, ExcelType.XLSX);
        }
        else if(outputPath.EndsWith(".csv")){
            Excel.ToExcelFile<object>(outputPath, sheetName, ExcelType.CSV);
        }
        else if(outputPath.EndsWith(".json")){
            Json.ToJsonFileAsync(data, outputPath);
        }
        else if(outputPath.EndsWith(".yaml")){
            Yaml.ToYamlFileAsync(data, outputPath);
        }
        else{
            throw new Exception("dont provide this output file format");
        }
    }

    internal static void ConvertAllFile(string inputPath, string outputDir, string outputType){
        outputDir = $"{CurrentDir}\\{outputDir}";
        inputPath = $"{CurrentDir}\\{inputPath}";

        ExcelType type = ExcelType.UNKNOWN;
        if(inputPath.EndsWith(".xlsx"))
            type = ExcelType.XLSX;
        else if(inputPath.EndsWith(".csv"))
            type = ExcelType.CSV;
        
        var sheetNames = MiniExcel.GetSheetNames(inputPath);

        if(outputType == "json"){
            foreach (var sheetName in sheetNames)
            {
                List<Dictionary<string, object>> data = Excel.FromExcelFile(inputPath, sheetName, type);
                var outputFile = $"{outputDir}\\{sheetName}.json";
                Json.ToJsonFileAsync(data, outputFile);
            }
        }
        else if(outputType == "yaml"){
            foreach (var sheetName in sheetNames)
            {
                List<Dictionary<string, object>> data = Excel.FromExcelFile(inputPath, sheetName, type);
                var outputFile = $"{outputDir}\\{sheetName}.yaml";
                Yaml.ToYamlFileAsync(data, outputFile);
            }
        }else{
            throw new Exception("dont provide this output file format");
        }
    }
}