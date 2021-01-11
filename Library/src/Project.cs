//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library
{
    public class Project
    {
        //================================
        // Properties
        //================================
        public ElementTable ElementTable { get; } = new ElementTable();
        public SourceTable SourceTable { get; } = new SourceTable();
        public ModuleTable ModuleTable { get; } = new ModuleTable();
        public DocumentTable DocumentTable { get; } = new DocumentTable();
        public CatalogTable CatalogTable { get; } = new CatalogTable();

        public TargetDictionar Targets { get; } = new TargetDictionar();


        //================================
        // Override Methods
        //================================
        public override string ToString()
        {
            var builder = new InformationBuilder();

            builder.AppendLine("[Sources]");
            foreach (var s in this.SourceTable)
            {
                builder.Append(s);
            }

            builder.AppendLine("[Moduleds]");
            foreach (var m in this.ModuleTable)
            {
                builder.Append(m);
            }

            builder.AppendLine("[Documents]");
            foreach (var d in this.DocumentTable)
            {
                builder.Append(d);
            }

            return builder.ToString();
        }


        //================================
        // Methods
        //================================
        public void AddElement(Element element)
        {
            this.ElementTable.Add(element);

            switch (element)
            {
                case Source s:
                    this.SourceTable.Add(s);
                    break;

                case Module m:
                    this.ModuleTable.Add(m);
                    break;

                case Document d:
                    this.DocumentTable.Add(d);
                    break;
            }

            return;
        }

        public void AddCatalog(Catalog catalog)
        {
            this.CatalogTable.Add(catalog.Id, catalog);
        }

        public void CreateTarget()
        {
            foreach (var document in this.DocumentTable)
            {
                if (this.CatalogTable.Contains(document.SourceId))
                {
                    var target = Target.Create(document, this.CatalogTable[document.SourceId]);
                    this.Targets.Add(document.Id, target);
                }
                else
                {
                    throw new NotesvelException("Unknon source(" + document.SourceId + ") at " + document.Id + ".");
                }
            }

            foreach (var targetPair in this.Targets)
            {
                var target = targetPair.Value;
                Logger.T("++++ Targets" + targetPair.Key);
                foreach (var item in target.Items)
                {
                    Logger.T("Name=" + item.Content.Name + ", Path=" + item.Path);
                }                
            }
        }

        public void BuildModule()
        {
            this.ModuleTable.Build();
        }
    }
}
