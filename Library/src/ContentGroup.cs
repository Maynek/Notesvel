//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library
{
    public class ContentGroup : Content
    {
        //================================
        // Properties
        //================================
        public override bool IsEntity { get { return false; } }


        //================================
        // Override Methods
        //================================
        public override string Read()
        {
            var value = string.Empty;

            foreach (var child in this.GetChildContents())
            {
                value += child.Read();
            }

            return value;
        }
    }
}
