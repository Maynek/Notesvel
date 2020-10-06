//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.IO;
using System.Xml;

namespace Maynek.Notesvel.Library.Xml
{
    public class XmlCatalogLoader : XmlLoaderBase<Catalog>
    {
        //================================
        // Properties
        //================================
        public Catalog Catalog { get { return this.Target; } }


        //================================
        // Methods
        //================================
        protected override void ParseDocument(XmlDocument document)
        {
            var element = document.DocumentElement;

            this.Catalog.Name = element.GetAttribute("name");
            this.ParseNotesvelItem(this.Catalog.Items, Catalog.MinLevel, element);
        }

        private bool ParseNotesvelItem(CatalogItemList list, int level, XmlElement element)
        {
            foreach (var childNode in element.ChildNodes)
            {
                if (childNode is XmlElement childElement)
                {
                    switch (childElement.Name)
                    {
                        case "group":
                            var group = this.GetGroup(level, childElement);
                            if (group != null)
                            {
                                list.Add(group);
                                this.ParseNotesvelItem(group.Items, level + 1, childElement);
                            }
                            break;

                        case "contents":
                            var contents = this.GetContents(level, childElement);
                            if (contents != null)
                            {
                                list.Add(contents);
                            }
                            break;
                    }

                }
            }

            return true;
        }

        private Group GetGroup(int level, XmlElement element)
        {
            var group = new Group();
            group.Level = level;

            if (int.TryParse(element.GetAttribute("index"), out int index))
            {
                group.Index = index;
            }
            else
            {
                return null;
            }

            group.Name = element.GetAttribute("name");
            if (group.Name == string.Empty)
            {
                group.Name = group.Index.ToString();
            }

            return group;
        }

        private Contents GetContents(int level, XmlElement element)
        {
            var contents = new Contents();
            contents.Level = level;
            if (int.TryParse(element.GetAttribute("index"), out int index))
            {
                contents.Index = index;
            }
            else
            {
                return null;
            }

            contents.Name = element.GetAttribute("name");
            if (contents.Name == string.Empty)
            {
                contents.Name = contents.Index.ToString();
            }

            contents.File = element.GetAttribute("file");
            if (contents.File == string.Empty)
            {
                return null;
            }

            return contents;
        }

        protected override void CorrectTarget() { }

    }
}
