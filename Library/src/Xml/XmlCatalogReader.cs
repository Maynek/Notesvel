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
    public static class XmlCatalogReaderExtension
    {
        public static bool SetCommonProperty(this Content content, XmlElement element)
        {
            content.Id = element.GetAttribute("Id");
            content.Name = element.GetAttribute("Name");

            if (content is PlainText text)
            {
                text.Path = element.GetAttribute("Path");
                if (text.Path == string.Empty)
                {
                    return false;
                }

            }
            return true;
        }
    }

    public class XmlCatalogReader : XmlReaderBase<Catalog>
    {
        public const string EXCEPTION_SOURCE_ID_IS_EMPTY = "XmlCatalogReader.SourceIdIsEmpty";
        public const string EXCEPTION_ID_IS_EMPTY = "XmlCatalogReader.IdIsEmpty";

        //================================
        // Properties
        //================================
        public string SourceId { get; set; } = string.Empty;


        //================================
        // Override Methods
        //================================
        protected override void ParseDocument(XmlDocument document)
        {
            if (this.SourceId == null || this.SourceId == string.Empty)
            {
                throw new NotesvelInternalException(
                    EXCEPTION_SOURCE_ID_IS_EMPTY,
                    "Source id is empty."
                );
            }

            this.Target.Id = this.SourceId;
            this.Target.Directory = Path.GetDirectoryName(this.FilePath);

            var element = document.DocumentElement;
            this.ParseContent(element, this.Target);

            return;
        }


        //================================
        // Methods
        //================================
        private void ParseContent(XmlElement element, Content parentContent)
        {
            if (parentContent == null)
            {
                return;
            }

            var index = 1;
            foreach (var childElement in element.ChildNodes.OfType<XmlElement>())
            {
                Content content = childElement.Name switch
                {
                    "Group"    => this.ParseGroup(childElement),
                    "PlainText" => this.ParsePlainText(childElement),
                    "NotesvelText" => this.ParseNotesvelText(childElement),
                    "Markdown" => this.ParseMarkdown(childElement),
                    _ => null,
                };

                if (content != null)
                {
                    parentContent.AddChildContent(content);
                    if (! content.IsEntity)
                    {
                        this.ParseContent(childElement, content);
                    }
                    index++;
                }
            }

            return;
        }

        private ContentGroup ParseGroup(XmlElement element)
        {
            var group = new ContentGroup();

            group.SetCommonProperty(element);

            return group;
        }

        private PlainText ParsePlainText(XmlElement element)
        {
            var plainText = new PlainText();

            if (!plainText.SetCommonProperty(element))
            {
                return null;
            }

            return plainText;
        }

        private NotesvelText ParseNotesvelText(XmlElement element)
        {
            var notesvelText = new NotesvelText();

            if (!notesvelText.SetCommonProperty(element))
            {
                return null;
            }

            return notesvelText;
        }

        private Markdown ParseMarkdown(XmlElement element)
        {
            var markdown = new Markdown();

            if (!markdown.SetCommonProperty(element))
            {
                return null;
            }

            return markdown;
        }
    }
}
