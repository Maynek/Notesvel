//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Maynek.Notesvel.Library
{
    public class Operation
    {
        //================================
        // Properties
        //================================
        public readonly Project Owner = null;
        public readonly string Id;
        public string DestinationDirectory { get; set; }

        //================================
        // Constructor
        //================================
        private Operation() { }
        public Operation(Project owner, string id)
        {
            this.Owner = owner;
            this.Id = id;
        }
    }
}
