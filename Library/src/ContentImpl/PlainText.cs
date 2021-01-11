//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library.ContentImpl
{
    public class PlainText : Content
    {
        //================================
        // Properties
        //================================
        public string Path { get; set; } = string.Empty;
        public string WorkFile { get; set; }


        //================================
        // Override Methods
        //================================
        public override string Read()
        {
            var path = FileUtility.CombinePath(this.Owner.Directory, this.Path);
            return FileUtility.ReadFile(path);
        }

        public override void AppendInformation(InformationBuilder builder)
        {
            base.AppendInformation(builder);
            builder.AppendIndentedLine("Path=" + this.Path);
        }
    }
}
