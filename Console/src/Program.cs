//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using Maynek.Command;
using Maynek.Notesvel.Library;
using Maynek.Notesvel.Library.Executers;
using Maynek.Notesvel.Library.Xml;

namespace Maynek.Notesvel.Console
{
    class Program
    {
        static readonly Writer Writer = new Writer() { EnabledWrite = true };

        static int Main(string[] args)
        {
            // Get commandline parameters.
            var parameter = Parameter.CreateParameter(args);
            if (parameter == null) return 1;

            Writer.EnabledDetail = parameter.ViewDetail;

            Print(LogLevel.Information, "-------- Parameter --------");
            Print(LogLevel.Information, parameter?.ToString());


            // Setup Notevel's Logger.
            var logger = new Logger();
            logger.Handler += Print;
            Logger.SetInstance(logger);

            // Setup Notesvel's Builder.
            var builder = new XmlBuilder()
            {
                ProjectFile = parameter.Project,
                ProjectSchema = parameter.ProjectSchema,
                CatalogSchema = parameter.CatalogSchema
            };
            builder.Executers.Add(new CommonExecuter("narou"));

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
            if (value != null)
            {
                switch (level)
                {
                    case LogLevel.Critical:
                    case LogLevel.Error:
                    case LogLevel.Warning:
                        Writer.Write(value);
                        break;

                    case LogLevel.Information:
                        Writer.WriteDetail(value);
                        break;

                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        Writer.WriteDebug(value);
                        break;
                }
            }
        }

    }
}
