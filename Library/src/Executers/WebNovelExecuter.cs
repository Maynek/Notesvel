//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using Maynek.Notesvel.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library.Executers
{
    public class WebNovelExecuter : ExecuterBase
    {
        private class TextFormatter
        {
            private readonly StringBuilder Builder = new StringBuilder();
            private readonly Dictionary<string, string> Brackets = new Dictionary<string, string>();

            public TextFormatter()
            {
                this.Brackets.Add("「", "」");
                this.Brackets.Add("（", "）");
            }

            public void Reset()
            {
                this.Builder.Clear();
            }

            private string GetStringSafe(string text, int startIndex, int length)
            {
                if (startIndex > text.Length - 1)
                {
                    return String.Empty;
                }

                if (startIndex + length > text.Length)
                {
                    return String.Empty;
                }

                return text.Substring(startIndex, length);
            }

            private bool JudgeNewLineSafe(string text, int index)
            {
                var s = this.GetStringSafe(text, index, 1);
                return (s == ExecuterBase.Newline);
            }

            private void AppendTextSafe(string text, int index, int length)
            {
                if (length < 1)
                {
                    return;
                }

                if (index + length > text.Length)
                {
                    return;
                }

                this.Builder.Append(text.Substring(index, length));
            }
            
            public string Replace(string text)
            {
                this.Reset();

                int index = 0;
                int startIndex = 0;

                while (index < text.Length)
                {
                    var s = text.Substring(index, 1);

                    if (this.Brackets.ContainsKey(s))
                    {
                        int closingIndex = text.IndexOf(this.Brackets[s], index);
                        if (closingIndex > index)
                        {
                            this.AppendTextSafe(text, startIndex, closingIndex - startIndex + 1);
                            index = closingIndex + 1;
                            startIndex = index;
                            continue;
                        }
                        else
                        {
                            index++;
                            continue;
                        }
                    }

                    if (s == "。")
                    {
                        this.AppendTextSafe(text, startIndex, index - startIndex + 1);
                        if (!this.JudgeNewLineSafe(text, index + 1))
                        {
                            this.Builder.Append(ExecuterBase.Newline);
                            this.Builder.Append("　");
                        }
                        index++;
                        startIndex = index;
                        continue;
                    }
                    
                    if (s == ExecuterBase.Newline)
                    {
                        this.AppendTextSafe(text, startIndex, index - startIndex);
                        
                        if (!this.JudgeNewLineSafe(text, index + 1))
                        {
                            this.Builder.Append(ExecuterBase.Newline);
                        }

                        if (index < text.Length - 1)
                        {
                            this.Builder.Append(ExecuterBase.Newline);
                        }
                        index++;
                        startIndex = index;
                        continue;
                    }

                    index++;
                }

                var lastText = text.Substring(startIndex);
                this.Builder.Append(lastText);

                return this.Builder.ToString();
            }
        }

        //================================
        // Fields
        //================================
        private TextFormatter Formatter = new TextFormatter();


        //================================
        // Constructor
        //================================
        private WebNovelExecuter() { }
        public WebNovelExecuter(string id) : base()
        {
            this.Id = id;
        }


        //================================
        // Methods
        //================================
        protected override string EditContents(string text)
        {          
            //Replace Tag.
            text = this.TagRegex.Replace(text, this.TagEvaluator);

            //Format.
            text = this.Formatter.Replace(text);

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