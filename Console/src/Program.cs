//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Maynek.Command;
using Maynek.Notesvel.Library;

namespace Maynek.Notesvel.Console
{
    class Program
    {
        static readonly Writer Writer = new Writer() { EnabledWrite = true };

        static int Main(string[] args)
        {
            var param = GetParameter(args);
            if (param == null) return 1;

            var logger = new Logger();
            logger.Handler += Print;

            var setting = GetProject(param, logger);
            if (setting == null) return 1;

            var catalog = GetCatalog(param, setting, logger);
            if (catalog == null) return 1;

            if (!Run(setting, catalog, logger)) return 1;

            return 0;
        }

        static Parameter GetParameter(string[] args)
        {
            var parameter = Parameter.CreateParameter(args);
            if (parameter == null) return null;

            Writer.EnabledDetail = parameter.ViewDetail;

            foreach (string message in parameter.ParseMessageList)
            {
                Print(LogLevel.Information, message);
            }

            Print(LogLevel.Trace, "[Parameter]");
            Print(LogLevel.Trace, parameter?.ToString());

            return parameter;
        }

        static Project GetProject(Parameter param, Logger logger)
        {
            Project project = null;

            if (File.Exists(param.Project))
            {
                var loader = new XmlProjectLoader()
                {
                    Logger = logger,
                    TargetPath = param.Project,
                    SchemaPath = param.ProjectSchema
                };

                try
                {
                    project = loader.Load();
                }
                catch (Exception)
                {
                    return null;
                }
                
                if (project != null)
                {
                    Print(LogLevel.Trace, "[Project]");
                    Print(LogLevel.Trace, project?.ToString());
                }
            }                 

            return project;
        }

        static Catalog GetCatalog(Parameter param, Project setting, Logger logger)
        {
            Catalog catalog = null;

            if (File.Exists(setting.CatalogFilePath))
            {
                var loader = new XmlCatalogLoader()
                {
                    Logger = logger,
                    TargetPath = setting.CatalogFilePath,
                    SchemaPath = param.CatalogSchema,
                };

                catalog = loader.Load();

                Print(LogLevel.Trace, "[Catalog]");
                Print(LogLevel.Trace, catalog?.ToString());
            }

            return catalog;
        }

        static bool Run(Project setting, Catalog catalog, Logger logger)
        {
            var builder = new Builder()
            {
                Logger = logger,
                Setting = setting,
                Catalog = catalog
            };
            builder.Run();

            return true;
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
