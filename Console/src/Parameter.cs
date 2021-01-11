//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Maynek.Command;
using Maynek.Notesvel.Library.Xml;

namespace Maynek.Notesvel.Console
{
    class Parameter
    {
        private const string DEFAULT_PROJECT_SCHEMA = "Project.xsd";
        private const string DEFAULT_CATALOG_SCHEMA = "Catalog.xsd";

        //================================
        // Properties
        //================================
        public List<string> ParseMessageList { get; } = new List<string>();
        public string SettingRoot { get; private set; } = string.Empty;
        public string ProjectFileName { get; private set; } = string.Empty;
        public string OutputDirectory { get; private set; } = string.Empty;
        public Variables Variables { get; } = new Variables();
        public string ProjectSchema { get; private set; } = string.Empty;
        public string CatalogSchema { get; private set; } = string.Empty;
        public bool ViewDetail { get; set; } = false;
        
        public string LogLevel { get; set; } = string.Empty;
        public string Intermediates { get; set; } = string.Empty;
        public bool SkipSchemeValidation { get; set; } = false;


        //================================
        // Methods
        //================================
        public static Parameter CreateParameter(string[] args)
        {
            var parameter = new Parameter();
            
            var parser = new Parser();

            //-------- Setting -------- 
            parser.ArgumentEvent += delegate (object sender, ArgumentEventArgs e)
            {
                if (args.Length > 0)
                {
                    parameter.ProjectFileName = e.Args[0];
                }
            };

            parser.AddOptionDefinition(new OptionDefinition("-o", "--output")
            {
                Type = OptionType.RequireValue,
                EventHandler = delegate (object sender, OptionEventArgs e)
                {
                    parameter.OutputDirectory = e.Value;
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("-v", "--view-detail")
            {
                Type = OptionType.NoValue,
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    parameter.ViewDetail = true;
                }                
            });

            parser.AddOptionDefinition(new OptionDefinition("--variable")
            {
                Type = OptionType.RequireValue,
                EventHandler = delegate (object sender, OptionEventArgs e)
                {
                    var index = e.Value.IndexOf('=');
                    if (index > -1)
                    {
                        var key = e.Value.Substring(0, index);
                        var value = string.Empty;
                        if (index < e.Value.Length)
                        {
                            value = e.Value.Substring(index + 1);
                        }
                        parameter.Variables.Add(key, value);
                    }
                    parameter.ViewDetail = true;
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("--log-level")
            {
                Type = OptionType.RequireValue,
                EventHandler = delegate (object sender, OptionEventArgs e)
                {
                    parameter.LogLevel = e.Value;
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("--intermediates")
            {
                Type = OptionType.RequireValue,
                EventHandler = delegate (object sender, OptionEventArgs e)
                {
                    parameter.Intermediates = e.Value;
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("--skip-scheme-validation")
            {
                Type = OptionType.NoValue,
                EventHandler = delegate (object sender, OptionEventArgs e)
                {
                    parameter.SkipSchemeValidation = true;
                }
            });

            parser.AddOptionDefinition(new OptionDefinition("-s", "--setting-root")
            {
                Type = OptionType.RequireValue,
                EventHandler = delegate (object sender, OptionEventArgs e)
                {
                    parameter.SettingRoot = e.Value;
                }
            });


            //-------- Parse -------- 
            parser.Parse(args);

            //-------- Correct -------- 
            if (parameter.SettingRoot == String.Empty)
            {
                parameter.SettingRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
  
            if (parameter.ProjectSchema == String.Empty)
            {
                parameter.ProjectSchema = Path.Combine(parameter.SettingRoot, DEFAULT_PROJECT_SCHEMA);
            }

            if (parameter.CatalogSchema == String.Empty)
            {
                parameter.CatalogSchema = Path.Combine(parameter.SettingRoot, DEFAULT_CATALOG_SCHEMA);
            }

            return parameter;
        }

        public override string ToString()
        {
            var b = new StringBuilder();

            b.AppendLine("SettingRoot=" + this.SettingRoot);
            b.AppendLine("Project=" + this.ProjectFileName);
            b.AppendLine("ProjectSchema=" + this.ProjectSchema);
            b.AppendLine("CatalogSchema=" + this.CatalogSchema);
            b.AppendLine("ViewDetail=" + this.ViewDetail.ToString());

            b.AppendLine("DebugLevel=" + this.LogLevel);
            b.AppendLine("SkipSchemeValidation=" + this.SkipSchemeValidation.ToString());

            b.AppendLine("Variables:");
            foreach (var p in this.Variables)
            {
                b.AppendLine("Key=" + p.Key + ", Value=" + p.Value );
            }

            return b.ToString();
        }
    }
}
