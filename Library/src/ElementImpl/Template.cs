//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Collections.Generic;

namespace Maynek.Notesvel.Library.ContentImpl
{
    public class ReplaceCommands : Dictionary<string, string> { }
    
    public class Template : Module, IFileElement
    {
        //================================
        // Properties
        //================================
        public ReplaceCommands ReplaceCommands = new ReplaceCommands();


        //================================
        // Constructor
        //================================
        private Template() { }


        //================================
        // Static Methods
        //================================
        public static Template Create(string id)
        {
            var instance = new Template();
            instance.Id = instance.ValidatedId(id);

            return instance;
        }


        //================================
        // Override Methods
        //================================
        public override void AppendInformation(InformationBuilder builder)
        {
            base.AppendInformation(builder);
            builder.AppendIndentedLine("FilePath=" + this.Path);

            foreach (var p in this.ReplaceCommands)
            {
                builder.AppendIndentedLine("To=" + p.Key + ", Value= " + p.Value);
            }
        }

        protected override string BuildValue(ModuleTable moduleTable)
        {
            var value = string.Empty;

            //Set self value.
            if (this.HasBase)
            {
                value = moduleTable[this.BaseId].GetValue();
                foreach (var p in this.ReplaceCommands)
                {
                    var oldValue = "{$" + p.Key + "}";
                    value = value.Replace(oldValue, this.OriginalValue);
                }
            }
            else
            {
                value = this.OriginalValue;
            }

            return value;
        }


        //================================
        // Methods
        //================================
        public void AddCommand(string to, string value)
        {
            this.ReplaceCommands.Add(to, value);
        }
    }
}
