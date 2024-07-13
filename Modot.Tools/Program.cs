
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Data;
using MiniExcelLibs;
using Modot.Persistence;

namespace Modot.Tools;
internal class Program
{
    static async Task<int> Main(string[] args)
    {
        //modot convert --excel  xxx --json xxx
        //
        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "An option whose argument is parsed as a FileInfo",
            isDefault: true,
            parseArgument: result =>
            {
                if (result.Tokens.Count == 0)
                {
                    return new FileInfo("sampleQuotes.txt");

                }
                string? filePath = result.Tokens.Single().Value;
                if (!File.Exists(filePath))
                {
                    result.ErrorMessage = "File does not exist";
                    return null;
                }
                else
                {
                    return new FileInfo(filePath);
                }
            });

        var delayOption = new Option<int>(
            name: "--delay",
            description: "Delay between lines, specified as milliseconds per character in a line.",
            getDefaultValue: () => 42);

        var fgcolorOption = new Option<ConsoleColor>(
            name: "--fgcolor",
            description: "Foreground color of text displayed on the console.",
            getDefaultValue: () => ConsoleColor.White);

        var lightModeOption = new Option<bool>(
            name: "--light-mode",
            description: "Background color of text displayed on the console: default is black, light mode is white.");

        var searchTermsOption = new Option<string[]>(
            name: "--search-terms",
            description: "Strings to search for when deleting entries.")
            { IsRequired = true, AllowMultipleArgumentsPerToken = true };

        var quoteArgument = new Argument<string>(
            name: "quote",
            description: "Text of quote.");

        var bylineArgument = new Argument<string>(
            name: "byline",
            description: "Byline of quote.");

        var rootCommand = new RootCommand("Modot tool for System.CommandLine");
        rootCommand.AddGlobalOption(fileOption);

        var quotesCommand = new Command("quotes", "Work with a file that contains quotes.");
        rootCommand.AddCommand(quotesCommand);

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

        var convertCommand = new Command("convert", "convert different data format file."){
            inputPathOption,
            outputPathOption,
            sheetNameOption
        };
        rootCommand.AddCommand(convertCommand);

        convertCommand.SetHandler(ConvertFile, inputPathOption, outputPathOption, sheetNameOption);

        return await rootCommand.InvokeAsync(args);
    }

    internal static void ConvertFile(string inputPath, string outputPath, string sheetName){
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
}