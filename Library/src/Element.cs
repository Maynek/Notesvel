//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library
{
    public interface IFileElement
    {
        public string Path { get; set; }
    };

    public abstract class ElementTableBase<T> : Table<string, T> where T : Element
    {
        public void Add(T value)
        {
            this.Add(value.Id, value);
        }
    }

    public class ElementTable : ElementTableBase<Element> { }

    public delegate string ReadValueHandler(Element element);

    public abstract class Element : IMessageBuildable
    {
        public const string EXCEPTION_ID_IS_EMPTY = "Element.IdIsEmpty";
        public const string EXCEPTION_ID_IS_INVALID = "Element.IdIsInvalid";

        public static ReadValueHandler ReadValueHandler;

        //================================
        // Properties
        //================================
        public string Id { get; protected set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;


        //================================
        // Methods
        //================================               
        protected bool IsEmptyValue(string value)
        {
            return ((value == null) || (value == string.Empty));
        }

        protected string ValidatedId(string id)
        {
            if (this.IsEmptyValue(id))
            {
                throw new NotesvelInternalException(
                    EXCEPTION_ID_IS_EMPTY,
                    "Id is empyt."
                );
            }

            if (id.IndexOf('.') > -1)
            {
                throw new NotesvelInternalException(
                    EXCEPTION_ID_IS_INVALID,
                    "Id(" + id + ") is invalid."
                );
            }
            
            return id;
        }

        public string ReadValue()
        {
            return Element.ReadValueHandler(this);
        }

        public virtual void AppendInformation(InformationBuilder builder)
        {
            builder.AppendLine("*" + this.GetType().Name + "(" + this.Id + ")");
            builder.AppendIndentedLine("Name=" + this.Name);         
        }

        public virtual string GetExpandedPath()
        {
            return Expander.Expand(this.Path);
        }
    }
}
