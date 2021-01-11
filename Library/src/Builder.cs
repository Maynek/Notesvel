//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Collections.Generic;

namespace Maynek.Notesvel.Library
{
    public abstract class Builder
    {
        //================================
        // Internal Classes
        //================================
        public class ExecuterDictionay : Dictionary<string, Executer>
        {
            public void Add(Executer executer)
            {
                this.Add(executer.Id, executer);
            }
        }


        //================================
        // Fields
        //================================
        public ExecuterDictionay Executers { get; private set; } = new ExecuterDictionay();


        //================================
        // Properties
        //================================
        public Project Project { get; private set; } = null;


        //================================
        // Methods
        //================================
        protected abstract Project ReadProject();
        protected abstract Catalog ReadCatalog(Source source);
        protected abstract string ReadElementValue(Element element);
        protected abstract void WriteDocument(TargetItem item, string text);

        protected virtual void Initialize() { return; }
        protected virtual void ModuleBuilt() { return; }
        protected virtual void DocumentBuilt() { return; }
        protected virtual void Finished() { return; }

        public void Run()
        {
            Element.ReadValueHandler = this.ReadElementValue;

            //Initialize
            this.Initialize();

            // Read project.
            this.Project = this.ReadProject();
            if (this.Project == null)
            {
                throw new NotesvelException();
            }

            // Read sources.
            foreach (var s in this.Project.SourceTable)
            {
                var catalog = this.ReadCatalog(s);
                if (catalog != null)
                {
                    this.Project.AddCatalog(catalog);
                }
            }

            // Create target table.
            this.Project.CreateTarget();

            // Build modules.
            this.Project.BuildModule();
            this.ModuleBuilt();

            // Build target.
            foreach (var target in this.Project.Targets)
            {
                foreach (var item in target.Value.Items)
                {
                    var text = item.Content.Read();
                    this.WriteDocument(item, text);
                }
            }
            this.DocumentBuilt();

            //Finish..
            this.Finished();

            return;
        }
    }
}

