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

namespace Maynek.Notesvel.Library
{
    public abstract class ExecuterBase
    {
        //================================
        // Constants
        //================================
        protected const string Newline = "\n";
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
        public string OutputNewline { get; set; } = Environment.NewLine;


        //================================
        // Methods
        //================================
        protected abstract string EditContents(string text);

        protected string ReplaceNewlineToInternal(string text)
        {
            return text.Replace("\r\n", Newline).Replace("\r", Newline).Replace("\n", Newline);
        }

        protected string ReplaceNewlineToOutput(string text)
        {
            return text.Replace(Newline, this.OutputNewline);
        }

        protected string ReadSource(string path)
        {
            if (! File.Exists(path))
            {
                return null;
            }

            string text = string.Empty;
            using (var reader = new StreamReader(path))
            {
                text = reader.ReadToEnd();
            }

            text = this.ReplaceNewlineToInternal(text);

            return text;
        }

        protected void WriteResult(string path, string text)
        {
            text = this.ReplaceNewlineToOutput(text);

            using (var writer = new StreamWriter(path))
            {
                writer.Write(text);
            }
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
            //Read source file.
            var srcFile = Path.Combine(operation.Owner.WorkDirectory, contents.WorkFile);
            var text = this.ReadSource(srcFile);
            if (text == null)
            {
                return;
            }

            text = this.EditContents(text);

            //Write result text.
            var dstFile = Path.Combine(dir, contents.WorkFile);
            dstFile = Path.ChangeExtension(dstFile, "txt");

            this.WriteResult(dstFile, text);
        }

    }
}