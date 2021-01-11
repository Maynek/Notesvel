//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library
{
    public class TargetDictionar : Dictionary<string, Target> { }

    public class TargetItem
    {
        public Document Document { get; set; } = null;
        public Content Content { get; set; } = null;
        public string Path { get; set; } = string.Empty;
    }

    public class Target
    {
        private class Selector
        {
            private readonly List<string> SelectorList = new List<string>();

            public int SelectLevel { get { return this.SelectorList.Count; } }

            public static Selector Create(string selectorString)
            {
                var instance = new Selector();
                if (selectorString != string.Empty)
                {
                    instance.SelectorList.AddRange(selectorString.Split("."));
                }

                return instance;
            }

            public bool Judge(int level, string id)
            {
                if (this.SelectorList.Count == 0)
                {
                    return true;
                }

                var currentSelector = this.SelectorList[level - 1];

                if (currentSelector == string.Empty || currentSelector == "?")
                {
                    return true;
                }

                if (currentSelector == id)
                {
                    return true;
                }

                return false;
            }
        }


        //================================
        // Properties
        //================================
        public Document Document { get; protected set; } = null;
        public List<TargetItem> Items = new List<TargetItem>();


        //================================
        // Static Methods
        //================================
        private static bool JudgeOutput(Content content, int level, int selectLevel)
        {
            if (selectLevel == 0)
            {
                return content.IsEntity;
            }

            if (level == selectLevel)
            {
                return true;
            }

            return false;
        }


        public static Target Create(Document document, Catalog catalog)
        {
            var instance = new Target();
            var selector = Selector.Create(document.Selector);

            instance.Document = document;

            instance.Select(catalog.GetChildContents(), selector, 1);

            return instance;
        }


        //================================
        // Methods
        //================================
        private void Select(ContentTable contentTable, Selector selector, int level)
        {           
            for (int i = 0; i < contentTable.Count; i++)
            {
                var content = contentTable[i];

                if (selector.Judge(level, content.Id))
                {
                    if (Target.JudgeOutput(content, level, selector.SelectLevel))
                    {
                        var newItem = new TargetItem();
                        newItem.Document = this.Document;
                        newItem.Content = content;
                        newItem.Path = this.Document.Path.Replace("$#", content.Label);
                        this.Items.Add(newItem);
                    }
                    else
                    {
                        this.Select(content.GetChildContents(), selector, level + 1);
                    }
                }
            }

            return;
        }
    }
}
