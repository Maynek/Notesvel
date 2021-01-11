//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Maynek.Notesvel.Library.Xml
{
    public abstract class XmlReaderBase<T> where T : class, new()
    {
        public const string EXCEPTION_OWNER_IS_NULL = "XmlReaderBase.OwnerIsNull";
        public const string EXCEPTION_PARSE_FAILED = "XmlReaderBase.ParseFailed";

        //================================
        // Properties
        //================================
        public T Target { get; private set; } = null;
        public XmlBuilder Owner { get; private set; } = null;
        public string FilePath { get; set; } = string.Empty;
        public string SchemaPath { get; set; } = string.Empty;
        public bool SkipSchemeValidation = false;

        protected List<NotesvelInternalException> ParseExeptionList = new List<NotesvelInternalException>();


        //================================
        // Methods
        //================================
        protected abstract void ParseDocument(XmlDocument document);

        public T Read(XmlBuilder owner)
        {
            if (owner == null)
            {
                throw new NotesvelInternalException(
                    EXCEPTION_OWNER_IS_NULL,
                    "Owner is null."
                );
            }
            this.Owner = owner;

            this.Target = new T();

            var document = this.LoadDocument();

            this.ParseDocument(document);
            if (this.ParseExeptionList.Count > 0)
            {
                foreach (var e in this.ParseExeptionList)
                {
                    Logger.E(e.Message);
                }
                throw new NotesvelInternalException(
                    EXCEPTION_PARSE_FAILED,
                    "Parsing Xml(" + this.FilePath + ") is failed."
                );
            }

            return this.Target;
        }

        protected XmlDocument LoadDocument()
        {
            if (this.FilePath == string.Empty)
            {
                Logger.E("File is not specified.");
                throw new NotesvelException("");
            }

            if (!File.Exists(this.FilePath))
            {
                Logger.E("File " + this.FilePath + "does not exist.");
                throw new NotesvelException("");
            }

            XmlDocument xmlDocument = new XmlDocument();

            if ( (!this.SkipSchemeValidation) && (this.SchemaPath != string.Empty))
            {
                var reader = new XmlTextReader(this.SchemaPath);
                try
                {
                    var schema = XmlSchema.Read(reader, null);
                    xmlDocument.Schemas.Add(schema);
                }
                catch (XmlException e)
                {
                    Logger.E("[ERROR] XML Schema Error.");
                    Logger.E("File : ");
                    Logger.E("  " + this.SchemaPath);
                    Logger.E("Message : ");
                    Logger.E("  " + e.Message);
                    throw new NotesvelException();
                }
            }

            try
            {
                xmlDocument.Load(this.FilePath);
            }
            catch (Exception e)
            {
                Logger.E("File \"" + this.FilePath + "\" open failed.");
                Logger.T(e.Message);
                throw new NotesvelException("");
            }

            if (xmlDocument.Schemas.Count > 0)
            {
                xmlDocument.Validate(delegate (object sender, ValidationEventArgs e)
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        Logger.E("Validate XML Errot. File = " + this.FilePath);
                        Logger.E(e.Message);
                        throw new NotesvelException("");
                    }
                });
            }

            return xmlDocument;
        }
    }
}
