//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Maynek.Command;

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
        public string SettingRoot { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string ProjectSchema { get; set; } = string.Empty;
        public string CatalogSchema { get; set; } = string.Empty;
        public bool ViewDetail { get; set; } = false;


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
                    parameter.Project = e.Args[0];
                }
            };

            parser.AddOptionDefinition(new OptionDefinition("-v", "--view-detail")
            {
                Type = OptionType.NoValue,
                EventHandler = delegate(object sender, OptionEventArgs e)
                {
                    parameter.ViewDetail = true;
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
            b.AppendLine("Project=" + this.Project);
            b.AppendLine("ProjectSchema=" + this.ProjectSchema);
            b.AppendLine("CatalogSchema=" + this.CatalogSchema);
            b.AppendLine("ViewDetail=" + this.ViewDetail.ToString());

            return b.ToString();
        }
    }
}
