//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library
{
    public interface IMessageBuildable
    {
        void AppendInformation(InformationBuilder builder);
    }

    public class InformationBuilder
    {
        private int Level = 0;
        private readonly List<object> Informations = new List<object>();

        //================================
        // Override Methods
        //================================
        public override string ToString()
        {
            var sb = new StringBuilder();
            this.Build(sb);
            return sb.ToString().TrimEnd('\r', '\n');
        }


        //================================
        // Methods
        //================================
        private void Build(StringBuilder builder)
        {
            foreach (var message in this.Informations)
            {
                if (message is string)
                {
                    builder.Append(message);
                }
                else if (message is InformationBuilder child)
                {
                    child.Build(builder);
                }
            }
        }

        public void Append(IMessageBuildable obj)
        {
            obj.AppendInformation(this);
        }

        public void AppendLine(string information)
        {
            this.AppendLineWithLevel(0, information);
        }

        public void AppendIndentedLine(string information)
        {
            this.AppendLineWithLevel(1, information);
        }

        public void AppendLineWithLevel(int level, string information)
        {
            this.Informations.Add(new string(' ', (this.Level + level) * 2) + information + Environment.NewLine);
        }

        public InformationBuilder CreateChild()
        {
            var builder = new InformationBuilder();
            builder.Level = this.Level + 1;
            this.Informations.Add(builder);
            return builder;
        }
    }
}
