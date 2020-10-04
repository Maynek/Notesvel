//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Maynek.Notesvel.Library
{
    public abstract class XmlLoaderBase<T> where T : class, new()
    {
        //================================
        // Properties
        //================================
        public Logger Logger { get; set; } = null;
        public T Target { get; private set; } = null;
        public string TargetPath { get; set; } = string.Empty;
        public string SchemaPath { get; set; } = string.Empty;


        //================================
        // Methods
        //================================
        public T Load()
        {
            this.Target = new T();

            var document = this.LoadDocument();
            this.ParseDocument(document);

            this.CorrectTarget();

            return this.Target;
        }

        protected XmlDocument LoadDocument()
        {
            if (this.TargetPath == string.Empty)
            {
                this.Logger?.E("Project file is not specified.");
                throw new NotesvelException("");
            }

            if (!File.Exists(this.TargetPath))
            {
                this.Logger?.E("Project file " + this.TargetPath + "does not exist.");
                throw new NotesvelException("");
            }

            XmlDocument document = new XmlDocument();

            if (this.SchemaPath != string.Empty)
            {
                var reader = new XmlTextReader(this.SchemaPath);
                try
                {
                    var schema = XmlSchema.Read(reader, null);
                    document.Schemas.Add(schema);
                }
                catch (XmlException e)
                {
                    this.Logger?.E("[ERROR] XML Schema Error.");
                    this.Logger?.E("File : ");
                    this.Logger?.E("  " + this.SchemaPath);
                    this.Logger?.E("Message : ");
                    this.Logger?.E("  " + e.Message);
                    throw new NotesvelException();
                }
            }

            try
            {
                document.Load(this.TargetPath);
            }
            catch (Exception)
            {
                this.Logger?.T(this.TargetPath);
                this.Logger?.E("Project file " + this.TargetPath + " open failed.");
                throw new NotesvelException("");
            }

            if (document.Schemas.Count > 0)
            {
                document.Validate(delegate (object sender, ValidationEventArgs e)
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        this.Logger?.E("Validate XML Errot. File = " + this.TargetPath);
                        this.Logger?.E(e.Message);
                        throw new NotesvelException("");
                    }
                });
            }

            return document;
        }

        protected abstract void ParseDocument(XmlDocument document);
        protected abstract void CorrectTarget();

    }
}
