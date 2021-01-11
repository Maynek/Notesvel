//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Maynek.Notesvel.Library.ContentImpl;
using Maynek.Notesvel.Library.ExecuterImpl;

namespace Maynek.Notesvel.Library.Xml
{
    public class Variables : Dictionary<string, string> { }

    public class XmlBuilder : Builder
    {
        //================================
        // Properties
        //================================
        public Variables Variables { get; } = new Variables();

        public string ProjectFilePath { get; set; } = string.Empty;
        public string ProjectDirectory { get; protected set; } = string.Empty;
        public string OutputDirectory { get; set; } = string.Empty;

        public string WorkDirectory { get; set; } = string.Empty;
        public string ModuleDirectory { get; set; } = string.Empty;

        public string ProjectSchemaPath { get; set; } = string.Empty;
        public string CatalogSchemaPath { get; set; } = string.Empty;

        public string Intermediates { get; set; } = string.Empty;
        public bool SkipSchemeValidation { get; set; } = false;


        //================================
        // Override Methods
        //================================
        protected override void Initialize()
        {
            // Setup enviroment of Notesvel.
            Expander.Handler = this.GetExpandedString;

            // Set properties of builder.
            this.ProjectDirectory = Path.GetDirectoryName(this.ProjectFilePath);
            this.OutputDirectory = FileUtility.CombinePath(this.ProjectDirectory, this.OutputDirectory);
            this.ModuleDirectory = FileUtility.CombinePath(this.ProjectDirectory, this.ModuleDirectory);
            this.WorkDirectory = FileUtility.CombinePath(this.ProjectDirectory, this.WorkDirectory);

            var sb = new StringBuilder();
            sb.AppendLine("-------- Builder --------");
            sb.AppendLine("ProjectDirectory=" + this.ProjectDirectory);
            sb.AppendLine("ProjectFilePath=" + this.ProjectFilePath);
            sb.AppendLine("OutputDirectory=" + this.OutputDirectory);
            sb.AppendLine("ModuleDirectory=" + this.ModuleDirectory);
            sb.AppendLine("WorkDirectory=" + this.WorkDirectory);

            Logger.D(sb.ToString());
        }

        protected override Project ReadProject()
        {
            // Read project file.
            var reader = new XmlProjectReader()
            {
                FilePath = this.ProjectFilePath,
                SchemaPath = this.ProjectSchemaPath,
                SkipSchemeValidation = this.SkipSchemeValidation,
            };

            var project = reader.Read(this);


            Logger.D("-------- Project --------");
            Logger.D(project.ToString());
            Logger.D("");

            return project;
        }

        protected override Catalog ReadCatalog(Source source)
        {
            Catalog catalog = null;

            if (source is CatalogFile catalogFile)
            {
                var filePath = FileUtility.CombinePath(this.ProjectDirectory, catalogFile.GetExpandedPath());

                Logger.T(filePath);

                // Read catalog from file.
                var reader = new XmlCatalogReader()
                {
                    SourceId = source.Id,
                    FilePath = filePath,
                    SchemaPath = this.CatalogSchemaPath,
                    SkipSchemeValidation = this.SkipSchemeValidation,
                };
                catalog = reader.Read(this);
            }

            if (catalog != null)
            {
                Logger.D("-------- Catalog --------");
                Logger.D(catalog.ToString());
                Logger.D("");
            }

            return catalog;
        }

        protected override string ReadElementValue(Element element)
        {
            var value = string.Empty;

            if (element is IFileElement fileElement)
            {
                var path = FileUtility.CombinePath(this.ProjectDirectory, fileElement.Path);
                value = FileUtility.ReadFile(path);
            }

            return value;
        }

        protected override void ModuleBuilt()
        {
            if (this.Intermediates != string.Empty)
            {
                var dir = this.Intermediates;

                if (!Path.IsPathRooted(dir))
                {
                    dir = Path.Combine(this.ProjectDirectory, dir);
                }

                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }

                Directory.CreateDirectory(dir);

                foreach (var m in this.Project.ModuleTable)
                {
                    var path = Path.Combine(dir, "module_" + m.Id + ".txt");
                    FileUtility.WriteFile(path, m.GetValue());
                }
            }

            return;
        }

        protected override void WriteDocument(TargetItem item, string text)
        {
            var executer = new NovelSiteExecuter("nv");

            text = executer.Execute(text);

            var path = FileUtility.CombinePath(this.ProjectDirectory, item.Path);

            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);

            FileUtility.WriteFile(path, text);
        }


        //================================
        // Methods
        //================================
        public void AddVariables(Variables variables)
        {
            if (variables != null)
            {
                foreach (var p in variables)
                {
                    if (this.Variables.ContainsKey(p.Key))
                    {
                        this.Variables[p.Key] = p.Value;
                    }
                    else
                    {
                        this.Variables.Add(p.Key, p.Value);
                    }
                }
            }

            return;
        }

        public string GetExpandedString(string key)
        {
            if (this.Variables.ContainsKey(key))
            {
                return this.Variables[key];
            }
            else
            {
                var value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
                if (value != null)
                {
                    return value;
                }
            }

            return string.Empty;
        }

    }
}
