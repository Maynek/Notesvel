//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library
{
    public abstract class CatalogItem
    {
        //================================
        // Properties
        //================================
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; } = 0;
        public int Index { get; set; } = 0;
        public bool IsContents { get { return (this is Contents); } }
        public CatalogItemList Items { get; } = new CatalogItemList();


        //================================
        // Methods
        //================================
        protected string GetSpace()
        {
            return new string(' ', this.Level * 2);
        }

        protected string ItemsToString()
        {
            var b = new StringBuilder();

            foreach (var item in this.Items)
            {
                b.Append(item.ToString());
            }

            return b.ToString();
        }
    }

    public class CatalogItemList : List<CatalogItem>
    {
        //================================
        // Methods
        //================================
        public new void Sort()
        {
            base.Sort((a, b) => a.Index - b.Index);
        }
    }

    public class Group : CatalogItem
    {
        //================================
        // Methods
        //================================
        public override string ToString()
        {
            var b = new StringBuilder();
            var space = this.GetSpace();

            b.AppendLine(space + "Group(Name=" + this.Name + ")");
            b.Append(this.ItemsToString());

            return b.ToString();
        }
    }

    public class Contents : CatalogItem
    {
        //================================
        // Properties
        //================================
        public string File { get; set; }
        public string WorkFile { get; set; }
        public new CatalogItemList Items { get; } = null;


        //================================
        // Methods
        //================================
        public override string ToString()
        {
            var b = new StringBuilder();
            var space = this.GetSpace();

            b.AppendLine(space + "Contents(Name=" + this.Name + ")");
            b.AppendLine(space + "  File=" + this.File);

            return b.ToString();
        }
    }
}
