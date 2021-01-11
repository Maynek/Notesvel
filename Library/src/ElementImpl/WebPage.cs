//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library.ContentImpl
{
    public class WebPage : Document
    {
        public const string EXCEPTION_CONTENT_IS_EMPTY = "ProjectElement.ContentIsEmpty";

        //================================
        // Properties
        //================================
        public string Url { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public string TemplateContent { get; set; } = string.Empty;


        //================================
        // Constructor
        //================================
        private WebPage() { }


        //================================
        // Static Methods
        //================================
        public static WebPage Create(string id, string sourceId)
        {
            var instance = new WebPage();
            instance.Id = instance.ValidatedId(id);
            if (instance.IsEmptyValue(sourceId))
            {
                throw new NotesvelInternalException(
                    EXCEPTION_CONTENT_IS_EMPTY,
                    "Content of WebPage(" + id + ") is empty."
                );
            }
            instance.SourceId = sourceId;

            return instance;
        }


        //================================
        // Override Methods
        //================================
        public override void AppendInformation(InformationBuilder builder)
        {
            base.AppendInformation(builder);
            builder.AppendIndentedLine("Url=" + this.Url);
            builder.AppendIndentedLine("TemplateId=" + this.TemplateId);
        }


        //================================
        // Methods
        //================================
        public string GetExpandedUrl()
        {
            return Expander.Expand(this.Url);
        }
    }
}
