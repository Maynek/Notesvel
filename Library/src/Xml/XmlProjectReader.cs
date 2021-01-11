//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.IO;
using System.Linq;
using System.Xml;
using Maynek.Notesvel.Library.ContentImpl;

namespace Maynek.Notesvel.Library.Xml
{
    public class XmlProjectReader : XmlReaderBase<Project>
    {
        //================================
        // Properties
        //================================
        public Project Project { get { return this.Target; } }


        //================================
        // Override Methods
        //================================
        protected override void ParseDocument(XmlDocument document)
        {
            var element = document.DocumentElement;

            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                switch (childElement.Name)
                {
                    case "Sources":
                        this.ParseSources(childElement);
                        break;

                    case "Modules":
                        this.ParseModules(childElement);
                        break;

                    case "Documents":
                        this.ParseDocuments(childElement);
                        break;
                }
            }

            return;
        }


        //================================
        // Methods
        //================================
        protected void AddElement(Element element)
        {
            if (element != null)
            {
                this.Project.AddElement(element);
            }
        }

        //-------- Source --------
        protected void ParseSources(XmlElement element)
        {
            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                try
                {
                    Source source = null;
                    switch (childElement.Name)
                    {
                        case "CatalogFile":
                            source = this.ParseCatalogFile(childElement);
                            break;
                    }
                    this.AddElement(source);
                }
                catch (NotesvelInternalException e)
                {
                    this.ParseExeptionList.Add(e);
                    continue;
                }
            }

            return;
        }

        protected CatalogFile ParseCatalogFile(XmlElement element)
        {
            var cf = CatalogFile.Create(
                element.GetAttribute("Id"),
                element.GetAttribute("Path")
            );

            return cf;
        }

        //-------- Module --------
        protected void ParseModules(XmlElement element)
        {
            var path = FileUtility.CombinePath(this.Owner.ModuleDirectory, element.GetAttribute("Path"));
            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                try
                {
                    Module module = childElement.Name switch
                    {
                        "Template" => this.ParseTemplate(childElement, path),
                        _ => null,
                    };
                    this.AddElement(module);
                }
                catch (NotesvelInternalException e)
                {
                    this.ParseExeptionList.Add(e);
                    continue;
                }
            }

            return;
        }

        protected Template ParseTemplate(XmlElement element, string moduleDir)
        {
            var id = element.GetAttribute("Id");
            
            var t = Template.Create(id);
            t.Path = Path.Combine(moduleDir + element.GetAttribute("Path"));
            t.Name = element.GetAttribute("Name");
            t.BaseId = element.GetAttribute("Base");

            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                switch (childElement.Name)
                {
                    case "Name":
                        t.Name = childElement.GetAttribute("Value");
                        break;

                    case "Insert":
                        if (childElement.HasAttribute("To"))
                        {
                            var to = childElement.GetAttribute("To");
                            t.AddCommand(to, "");
                        }
                        break;
                }
            }

            return t;
        }

        //-------- Document --------
        private void ParseDocuments(XmlElement element)
        {
            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                try
                {
                    Document document = childElement.Name switch
                    {
                        "NovelSite" => this.ParseNovelSite(childElement),
                        "WebPage" => this.ParseWebPage(childElement),
                        _ => null,
                    };
                    this.AddElement(document);
                }
                catch (NotesvelInternalException e)
                {
                    this.ParseExeptionList.Add(e);
                }
            }

            return;
        }

        private NovelSite ParseNovelSite(XmlElement element)
        {
            var novelSite = NovelSite.Create(
                element.GetAttribute("Id"),
                element.GetAttribute("Source")
            );

            novelSite.Selector = element.GetAttribute("Selector");
            novelSite.Path = element.GetAttribute("Path");

            return novelSite;
        }

        private WebPage ParseWebPage(XmlElement element)
        {
            var webPage = WebPage.Create(
                element.GetAttribute("Id"),
                element.GetAttribute("Source")
            );
            webPage.Selector = element.GetAttribute("Selector");
            webPage.Path = element.GetAttribute("Path");

            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                switch (childElement.Name)
                {
                    case "Name":
                        webPage.Name = element.GetAttribute("Value"); ;
                        break;

                    case "Url":
                        webPage.Url = element.GetAttribute("Path");
                        break;

                    case "Template":
                        webPage.TemplateId = element.GetAttribute("Module");
                        webPage.TemplateContent = element.GetAttribute("Content");
                        break;
                }
            }

            return webPage;
        }
    }
}
