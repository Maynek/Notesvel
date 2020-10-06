//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.IO;
using System.Xml;

namespace Maynek.Notesvel.Library.Xml
{
    public class XmlProjectLoader : XmlLoaderBase<Project>
    {
        //================================
        // Properties
        //================================
        public Project Project { get { return this.Target; } }


        //================================
        // Methods
        //================================
        protected override void ParseDocument(XmlDocument document)
        {
            var element = document.DocumentElement;

            foreach (var childNode in element.ChildNodes)
            {
                if (childNode is XmlElement)
                {
                    var childElement = (XmlElement)childNode;
                    switch (childElement.Name)
                    {
                        case "catalog":
                            this.Project.CatalogFile = childElement.InnerText;
                            break;

                        case "operation":
                            this.ParseOperation(childElement);
                            break;
                    }

                }
            }

            return;
        }

        private void ParseOperation(XmlElement element)
        {
            var id = element.GetAttribute("id");
            if (id != string.Empty)
            {
                Operation op;
                if (this.Project.Operations.ContainsKey(id))
                {
                    op = this.Project.Operations[id];
                }
                else
                {
                    op = new Operation(this.Project, id);
                    this.Project.Operations.Add(id, op);
                }

                foreach (var childNode in element.ChildNodes)
                {
                    if (childNode is XmlElement childElement)
                    {
                        switch (childElement.Name)
                        {
                            case "destination":
                                op.DestinationDirectory = childElement.InnerText;
                                break;
                        }
                    }
                }
            }
        }

        protected override void CorrectTarget()
        {
            if (this.Project == null) return;

            this.Project.ProjectFilePath = this.TargetPath;
            this.Project.RootDirectory = Path.GetDirectoryName(this.TargetPath);

            if (!Path.IsPathRooted(this.Project.WorkDirectory))
            {
                this.Project.WorkDirectory = Path.Combine(
                    this.Project.RootDirectory, this.Project.WorkDirectory);
            }

            if (!Path.IsPathRooted(this.Project.CatalogFile))
            {
                this.Project.CatalogFile = Path.Combine(
                    this.Project.RootDirectory, this.Project.CatalogFile);
            }

            this.Project.SourceDirectory = Path.GetDirectoryName(this.Project.CatalogFile);
        }

    }
}
