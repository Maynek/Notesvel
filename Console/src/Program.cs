//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using Maynek.Notesvel.Library;
using Maynek.Notesvel.Library.ExecuterImpl;
using Maynek.Notesvel.Library.Xml;

namespace Maynek.Notesvel.Console
{
    class Program
    {
        static Parameter Parameter;
        static LogLevel LogLevel = LogLevel.Warning;

        static int Main(string[] args)
        {
            // Get commandline parameters.
            Program.Parameter = Parameter.CreateParameter(args);
            if (Program.Parameter == null) return 1;

            if (!LogLevel.TryParse(Program.Parameter.LogLevel, out Program.LogLevel))
            {
                Program.LogLevel = LogLevel.Warning;
            }

            Print(LogLevel.Debug, "-------- Parameter --------");
            Print(LogLevel.Debug, Program.Parameter?.ToString());

            // Setup Notevel's Logger.
            Logger.LoggingHandler += Print;

            // Setup Notesvel's Builder.
            var builder = new XmlBuilder()
            {
                ProjectFilePath = Program.Parameter.ProjectFileName,
                OutputDirectory = Program.Parameter.OutputDirectory,
                ProjectSchemaPath = Program.Parameter.ProjectSchema,
                CatalogSchemaPath = Program.Parameter.CatalogSchema,
                Intermediates = Program.Parameter.Intermediates,
                SkipSchemeValidation = Program.Parameter.SkipSchemeValidation,
            };
            builder.AddVariables(Program.Parameter.Variables);
            builder.Executers.Add(new NovelSiteExecuter("narou"));

            // Run Notesvel's Builder.
            try
            {
                builder.Run();
            }
            catch (Exception e)
            {
                Print(LogLevel.Critical, e.Message);
                return 1;
            }
            
            return 0;
        }
       
        static void Print(LogLevel level, string value)
        {
            if (value == null)
            {
                return;
            }

            if (!Program.Parameter.ViewDetail && (level < LogLevel.Warning))
            {
                return;
            }
            else if (level < Program.LogLevel)
            {
                return;
            }

            System.Console.WriteLine(value);
        }
    }
}
