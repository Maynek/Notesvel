//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;

namespace Maynek.Notesvel.Library
{
    public class CatalogTable : Table<string, Catalog> { }


    public class Catalog : ContentGroup
    {
        //================================
        // Properties
        //================================
        public string Directory { get; set; } = String.Empty;


        //================================
        // Override Methods
        //================================
        public override string ToString()
        {
            var builder = new InformationBuilder();

            this.AppendInformation(builder);

            return builder.ToString();
        }

        public override void AppendInformation(InformationBuilder builder)
        {
            base.AppendMyInformation(builder);
            builder.AppendIndentedLine("Directory=" + this.Directory);
            base.AppendChildInformation(builder);
        }
    }
}
