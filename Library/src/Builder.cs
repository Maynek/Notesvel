//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Collections.Generic;
using System.IO;

namespace Maynek.Notesvel.Library
{
    public class Builder
    {
        //================================
        // Internal Classes
        //================================
        public class ExecuterDictionay : Dictionary<string, ExecuterBase>
        {
            public void Add(ExecuterBase executer)
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
        protected Logger Logger { get { return Logger.Instance; } }
        public Project Project { get; set; } = null;


        //================================
        // Methods
        //================================
        public virtual void OnLoadProject() { }

        public virtual void Run()
        {
            if (this.Project == null)
            {
                this.OnLoadProject();
            }

            if (this.Project == null) throw new NotesvelException();
            if (this.Project.Catalog == null) throw new NotesvelException();

            this.RunPreprocess();

            this.RunOperation();
        }
        
        //-------- Preprocess --------
        private void RunPreprocess()
        {
            if (!Directory.Exists(this.Project.WorkDirectory))
            {
                var info = Directory.CreateDirectory(this.Project.WorkDirectory);
                info.Attributes |= FileAttributes.Hidden;
            }

            this.RunPreprocessInternal(this.Project.Catalog.Items, "c");
        }

        private void RunPreprocessInternal(CatalogItemList items, string fileBase)
        {
            foreach (var item in items)
            {
                var newFileBase = fileBase + "_" + item.Index.ToString();

                if (item is Contents contents)
                {
                    contents.WorkFile = newFileBase + ".nvw";
                    this.CreateWorkFile(contents);
                }
                else
                {
                    this.RunPreprocessInternal(item.Items, newFileBase);
                }
            }
        }

        private void CreateWorkFile(Contents contents)
        {
            var contentsPath = Path.Combine(this.Project.SourceDirectory, contents.File);
            var workPath = Path.Combine(this.Project.WorkDirectory, contents.WorkFile);

            File.Copy(contentsPath, workPath, true);
        }

        //-------- Operation --------
        private void RunOperation()
        {
            foreach (var operation in this.Project.Operations.Values)
            {
                if (!this.Executers.ContainsKey(operation.Id))
                {
                    this.Logger?.W("Unkonw Operation. Id = " + operation.Id);
                    continue;
                }

                this.Executers[operation.Id].Execute(operation);
            }
        }
    }
}

