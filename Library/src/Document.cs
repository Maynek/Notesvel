//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library
{
    public class DocumentTable : ElementTableBase<Document> { }

    public abstract class Document : Element
    {
        //================================
        // Properties
        //================================
        public string SourceId { get; set; } = string.Empty;
        public string Selector { get; set; } = string.Empty;
    }
}
