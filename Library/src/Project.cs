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

            b.Append("RootDirectory=" + this.RootDirectory + Environment.NewLine);
            b.Append("SourceDirectory=" + this.SourceDirectory + Environment.NewLine);
            b.Append("WorkDirectory=" + this.WorkDirectory + Environment.NewLine);
            b.Append("ProjectFilePath=" + this.ProjectFilePath + Environment.NewLine);
            b.Append("CatalogFilePath=" + this.CatalogFile + Environment.NewLine);

            foreach (var op in this.Operations.Values)
            {
                b.Append("Operation(Id=" + op.Id + ")" + Environment.NewLine);
                b.Append("  DestinationDirectory=" + op.DestinationDirectory + Environment.NewLine);
            }
            
            return b.ToString();
        }
    }
}
