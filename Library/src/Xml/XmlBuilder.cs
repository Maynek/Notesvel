//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library.Xml
{
    public class XmlBuilder : Builder
    {
        //================================
        // Properties
        //================================
        public new Project Project { get { return base.Project; } }
        public string ProjectFile { get; set; } = string.Empty;
        public string ProjectSchema { get; set; } = string.Empty;
        public string CatalogSchema { get; set; } = string.Empty;


        //================================
        // Methods
        //================================
        public override void OnLoadProject()
        {
            // Load Notesvel's Project File.
            var projectLoader = new XmlProjectLoader()
            {
                TargetPath = this.ProjectFile,
                SchemaPath = this.ProjectSchema
            };
            base.Project = projectLoader.Load();

            this.Logger?.I("-------- Project --------");
            this.Logger?.I(this.Project.ToString());


            // Load Notesvel's Catalog File.
            var catalogLoader = new XmlCatalogLoader()
            {
                TargetPath = base.Project.CatalogFile,
                SchemaPath = this.CatalogSchema,
            };
            base.Project.Catalog = catalogLoader.Load();

            this.Logger?.I("-------- Catalog --------");
            this.Logger?.I(base.Project.Catalog.ToString());

        }
    }
}
