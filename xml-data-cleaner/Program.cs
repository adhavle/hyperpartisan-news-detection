using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Formats.Tar;

namespace xml_data_cleaner
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Option<FileInfo> xmlFileOption = new("--file")
            {
                Description = "The XML data file that needs cleaning.",
                Required = true,
            };

            RootCommand rootCommand = new("Utility for cleaning (delimiting) unwanted XML tags")
            {
                xmlFileOption,
            };

            ParseResult parseResult = rootCommand.Parse(args);

            if (parseResult.Errors.Count > 0)
            {
                ShowParamError(rootCommand, parseResult.Errors.Select(err => err.Message).ToList());
                return 1;
            }

            if (parseResult.GetValue(xmlFileOption) is FileInfo inputFile && inputFile.Exists)
            {
                Console.WriteLine($"{inputFile.Name} found!");
            }
            else
            {
                ShowParamError(rootCommand, ["Error opening file."]);
            }

            return 0;
        }

        private static void ShowParamError(RootCommand rootCommand, List<string> errorMessage)
        {

            foreach (var msg in errorMessage)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(msg);
                Console.ResetColor();
                Console.WriteLine("\n");
            }

            rootCommand.Parse("-h").Invoke();
        }
    }
}
