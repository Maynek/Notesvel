//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library.ContentImpl
{
    public class CatalogFile : Source, IFileElement
    {
        public const string EXCEPTION_PATH_IS_EMPTY = "Catalog.PathIsEmpty";


        //================================
        // Constructor
        //================================
        private CatalogFile() { }


        //================================
        // Static Methods
        //================================
        public static CatalogFile Create(string id, string path)
        {
            var instance = new CatalogFile();
            instance.Id = instance.ValidatedId(id);

            if (instance.IsEmptyValue(path))
            {
                throw new NotesvelInternalException(
                    EXCEPTION_PATH_IS_EMPTY,
                    "FileName of Catalog(" + id + ") is empty."
                );
            }
            instance.Path = path;

            return instance;
        }


        //================================
        // Override Methods
        //================================
        public override void AppendInformation(InformationBuilder builder)
        {
            base.AppendInformation(builder);
            builder.AppendIndentedLine("FilePath=" + this.Path);
        }
    }
}
