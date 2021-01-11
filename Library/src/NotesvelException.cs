//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;

namespace Maynek.Notesvel.Library
{
    public class NotesvelInternalException : Exception
    {
        public string ReasonId { get; protected set; } = string.Empty;

        public NotesvelInternalException(string reaseonId, string message) : base(message)
        {
            this.ReasonId = reaseonId;
        }
    }

    public class NotesvelException : Exception
    {
        public NotesvelException() : base() { }
        public NotesvelException(String message) : base(message) { }
    }
}
