//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;

namespace Maynek.Notesvel.Library
{
    public class NotesvelException : Exception
    {
        public NotesvelException() : base() { }
        public NotesvelException(String message) : base(message) { }
    }
}
