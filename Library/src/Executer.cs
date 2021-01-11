//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library
{
    public abstract class Executer
    {
        //================================
        // Constants
        //================================
        protected const string TagSearchPattern = @"<%(?<SCRIPT>.*?)%>";
        protected const string WordPattern = @"(?<WORD>.+?)\((?<RUBY>.*?)\)";
        protected const string WordReplace = @"｜${WORD}《${RUBY}》";
        protected const RegexOptions Options = RegexOptions.Singleline | RegexOptions.Compiled;


        //================================
        // Fields
        //================================
        protected readonly Regex TagRegex = new Regex(TagSearchPattern, Options);
        protected readonly Regex WordRegex = new Regex(WordPattern, Options);


        //================================
        // Properties
        //================================
        public string Id { get; protected set; }


        //================================
        // Methods
        //================================
        protected abstract string EditContents(string text);

        public string Execute(string text)
        {
            return this.EditContents(text);
        }
    }
}