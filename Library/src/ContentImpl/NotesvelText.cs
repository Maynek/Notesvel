//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library.ContentImpl
{
    public class NotesvelText : PlainText
    {
        //================================
        // Static Methods
        //================================
        protected static string PreProcess(string value)
        {
            value = value.Replace("--", "――");
            value = value.Replace("...", "……");
            return value;
        }


        //================================
        // Override Methods
        //================================
        public override string Read()
        {
            var value = base.Read();
            value = NotesvelText.PreProcess(value);

            return value;
        }
    }
}
