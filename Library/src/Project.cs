//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library
{
    public class Project
    {
        //================================
        // Constants
        //================================
        private const string DEFAULT_WORK_DIR = ".nv";


        //================================
        // Properties
        //================================
        public string RootDirectory { get; set; }
        public string SourceDirectory { get; set; }
        public string WorkDirectory { get; set; } = Project.DEFAULT_WORK_DIR;
        public string ProjectFilePath { get; set; }
        public string CatalogFile { get; set; }

        public Catalog Catalog { get; set; }
        public Dictionary<string, Operation> Operations { get; private set; }
            = new Dictionary<string, Operation>();


        //================================
        // Methods
        //================================
        public override string ToString()
        {
            var b = new StringBuilder();

            b.AppendLine("RootDirectory=" + this.RootDirectory);
            b.AppendLine("SourceDirectory=" + this.SourceDirectory);
            b.AppendLine("WorkDirectory=" + this.WorkDirectory);
            b.AppendLine("ProjectFilePath=" + this.ProjectFilePath);
            b.AppendLine("CatalogFilePath=" + this.CatalogFile);

            foreach (var op in this.Operations.Values)
            {
                b.AppendLine("Operation(Id=" + op.Id + ")");
                b.AppendLine("  DestinationDirectory=" + op.DestinationDirectory);
            }
            
            return b.ToString();
        }
    }
}
