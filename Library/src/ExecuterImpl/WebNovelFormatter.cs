//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library.ExecuterImpl
{
    public class WebNovelFormatter
    {
        //================================
        // Fields
        //================================
        private static WebNovelFormatter Instance;
        private readonly StringBuilder Builder = new StringBuilder();
        private readonly Dictionary<string, string> Brackets = new Dictionary<string, string>();


        //================================
        // Constructor
        //================================
        public WebNovelFormatter()
        {
            this.Brackets.Add("「", "」");
            this.Brackets.Add("（", "）");
        }


        //================================
        // Static Methods
        //================================
        public static WebNovelFormatter GetInstance()
        {
            if (WebNovelFormatter.Instance == null)
            {
                WebNovelFormatter.Instance = new WebNovelFormatter();
            }

            return WebNovelFormatter.Instance;
        }

        public static string Format(string text)
        {
            return WebNovelFormatter.GetInstance().FormatText(text);
        }


        //================================
        // Methods
        //================================
        private void Reset()
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
            return (s == FileUtility.NewLine);
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
            
        public string FormatText(string text)
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
                        this.Builder.Append(FileUtility.NewLine);
                        this.Builder.Append("　");
                    }
                    index++;
                    startIndex = index;
                    continue;
                }
                    
                if (s == FileUtility.NewLine)
                {
                    this.AppendTextSafe(text, startIndex, index - startIndex);
                      
                    if (!this.JudgeNewLineSafe(text, index + 1))
                    {
                        this.Builder.Append(FileUtility.NewLine);
                    }

                    if (index < text.Length - 1)
                    {
                        this.Builder.Append(FileUtility.NewLine);
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
}