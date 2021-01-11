//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library
{
    public class ContentTable : Table<string, Content>
    {
        public Content Owner { get; protected set; } = null;

        //================================
        // Constructor
        //================================
        public ContentTable(Content owner)
        {
            this.Owner = owner;
        }


        //================================
        // Methods
        //================================
        public void Sort()
        {
            base.Sort((a, b) => a.Index - b.Index);
        }
    }


    public abstract class Content : IMessageBuildable
    {
        //================================
        // Properties
        //================================
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Index { get; protected set; } = 1;

        public Catalog Owner { get; protected set; } = null;
        public Content Parent { get; protected set; } = null;
        protected ContentTable ChildContents { get; } = null;

        public virtual bool IsEntity { get { return true; } }
        public bool IsRoot { get { return (this.Parent == null); } }


        //================================
        // Methods
        //================================
        public abstract string Read();

        public Content()
        {
            this.ChildContents = new ContentTable(this);
        }

        public void UpdateLabel()
        {
            if (this.Parent == null)
            {
                this.Label = string.Empty;
            }
            else
            {
                this.Label = this.Parent.Label + "_" + this.Index.ToString();
            }
        }

        public void SetIndex(int index)
        {
            this.Index = index;
        }

        public ContentTable GetChildContents()
        {
            if (!this.IsEntity)
            {
                return this.ChildContents;
            }

            return null;
        }

        public void AddChildContent(Content child)
        {
            if (child != null && !this.IsEntity)
            {
                child.Owner = (this is Catalog catalog) ? catalog : this.Owner; 
                child.Parent = this;
                child.SetIndex(this.ChildContents.Count + 1);
                child.UpdateLabel();

                if (child.Id == string.Empty)
                {
                    child.Id = "#" + child.Index.ToString();
                }

                this.ChildContents.Add(child.Id, child);
            }
        }

        public virtual void AppendInformation(InformationBuilder builder)
        {
            this.AppendMyInformation(builder);
            this.AppendChildInformation(builder);
        }

        protected void AppendMyInformation(InformationBuilder builder)
        {
            builder.AppendLine("*" + this.GetType().Name + "(" + this.Id + ") Index=" + this.Index.ToString());
            builder.AppendIndentedLine("Label=" + this.Label);
            builder.AppendIndentedLine("Name=" + this.Name);
        }

        protected void AppendChildInformation(InformationBuilder builder)
        {
            if (!this.IsEntity)
            {
                var childBuilder = builder.CreateChild();
                foreach (var c in this.ChildContents)
                {
                    childBuilder.Append(c);
                }
            }
        }
    }
}
