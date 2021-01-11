//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library.ContentImpl
{
    public class NovelSite : Document
    {
        public const string EXCEPTION_CONTENT_IS_EMPTY = "ProjectElement.ContentIsEmpty";

        //================================
        // Constructor
        //================================
        private NovelSite() { }


        //================================
        // Static Methods
        //================================
        public static NovelSite Create(string id, string sourceId)
        {
            var instance = new NovelSite();
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
    }
}
