//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using Maynek.Notesvel.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library.Executers
{
    public class CommonExecuter : IExecuter
    {
        //================================
        // Constants
        //================================
        private const string TagSearchPattern = @"<%(?<SCRIPT>.*?)%>";
        private const RegexOptions TagSearchOption = RegexOptions.Singleline;
        private const string WordPattern = @"(?<WORD>.+?)\((?<RUBY>.*?)\)";
        private const string WordReplace = @"｜${WORD}《${RUBY}》";
        private const RegexOptions WordOption = RegexOptions.Singleline;


        //================================
        // Fields
        //================================
        private readonly Regex TagRegex = new Regex(TagSearchPattern, TagSearchOption);
        private readonly Regex WordRegex = new Regex(WordPattern, WordOption);


        //================================
        // Properties
        //================================
        private Logger Logger { get { return Logger.Instance; } }
        public readonly string Id;


        //================================
        // Constructor
        //================================
        private CommonExecuter() { }
        public CommonExecuter(string id)
        {
            this.Id = id;
        }


        //================================
        // Methods
        //================================
        public string GetId()
        {
            return this.Id;
        }

        public void Execute(Operation operation)
        {
            var dir = Path.Combine(operation.Owner.RootDirectory, operation.DestinationDirectory);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            this.ExecuteItem(operation, dir, operation.Owner.Catalog);
        }

        private void ExecuteItem(Operation operation, string dir, CatalogItem item)
        {
            if (item is Contents contents)
            {
                this.ExecuteContents(operation, dir, contents);
            }
            else
            {
                foreach (var childItem in item.Items)
                {
                    this.ExecuteItem(operation, dir, childItem);
                }
            }
        }

        private void ExecuteContents(Operation operation, string dir, Contents contents)
        {
            var srcText = string.Empty;
            var srcFile = Path.Combine(operation.Owner.WorkDirectory, contents.WorkFile);
            using (var reader = new StreamReader(srcFile))
            {
                srcText = reader.ReadToEnd();
            }

            var dstText = this.TagRegex.Replace(srcText, this.TagEvaluator);
            var dstFile = Path.Combine(dir, contents.WorkFile);
            dstFile = Path.ChangeExtension(dstFile, "txt");
            using (var writer = new StreamWriter(dstFile))
            {
                writer.Write(dstText);
            }
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