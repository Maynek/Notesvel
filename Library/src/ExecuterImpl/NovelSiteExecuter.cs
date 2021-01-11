//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library.ExecuterImpl
{
    public class NovelSiteExecuter : Executer
    {
        //================================
        // Constructor
        //================================
        private NovelSiteExecuter() { }
        public NovelSiteExecuter(string id) : base()
        {
            this.Id = id;
        }


        //================================
        // Methods
        //================================
        protected override string EditContents(string text)
        {
            //Format.
            text = WebNovelFormatter.Format(text);

            //Replace Tag.
            text = this.TagRegex.Replace(text, this.TagEvaluator);

            return text;
        }

        private string TagEvaluator(Match match)
        {
            return this.ResolveScript(match.Groups["SCRIPT"].ToString());
        }

        //-------- Resolve --------
        private string ResolveScript(string script)
        {
            return this.WordRegex.Replace(script, WordReplace);
        }
    }
}