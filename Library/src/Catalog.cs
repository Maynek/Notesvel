//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library
{
    public class Catalog : CatalogItem
    {
        //================================
        // Constants
        //================================
        public const int MinLevel = 1;
        

        //================================
        // Methods
        //================================
        public override string ToString()
        {
            var b = new StringBuilder();

            b.Append("Name=" + this.Name + Environment.NewLine);

            b.Append("Items" + Environment.NewLine);
            foreach (var item in this.Items)
            {
                b.Append(item.ToString());
            }

            return b.ToString();
        }
    }
}
