//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;

namespace Maynek.Command
{
    /// <summary>
    /// Writes strings to console.
    /// </summary>
    public class Writer
    {
        public bool EnabledWrite { get; set; } = false;
        public bool EnabledDetail { get; set; } = false;

        public void Write(string value)
        {
            if (this.EnabledWrite)
            {
                Console.WriteLine(value);
            }
        }

        public void WriteDetail(string value)
        {
            if (this.EnabledDetail) this.Write(value);
        }

        public void WriteDebug(string value)
        {
#if DEBUG
            if (this.EnabledDetail) this.Write(value);
#endif
        }
    }
}
