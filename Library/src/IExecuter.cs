//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using Maynek.Notesvel.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library
{
    public interface IExecuter
    {
        public string GetId();
        public void Execute(Operation operation);
    }
}