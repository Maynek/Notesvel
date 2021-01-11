//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.IO;

namespace Maynek.Notesvel.Library
{
    public static class FileUtility
    {
        public static string NewLine = "\n";
        public static string OutputNewLine { get; set; } = Environment.NewLine;

        public static string ReplaceNewlineToInternal(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        public static string ReplaceNewlineToOutput(string text)
        {
            return text.Replace("\n", FileUtility.OutputNewLine);
        }

        public static string ReadFile(string path)
        {
            string text = string.Empty;
            using (var reader = new StreamReader(path))
            {
                text = reader.ReadToEnd();
            }

            text = FileUtility.ReplaceNewlineToInternal(text);

            return text;
        }

        public static void WriteFile(string path, string text)
        {
            text = FileUtility.ReplaceNewlineToOutput(text);

            using (var writer = new StreamWriter(path))
            {
                writer.Write(text);
            }
        }

        public static string CombinePath(string path1, string path2)
        {
            var path = string.Empty;

            if (path2 == null || path2 == string.Empty)
            {
                path = path1;
            }
            else if (Path.IsPathRooted(path2))
            {
                path = path2;
            }
            else
            {
                path = Path.Combine(path1, path2);
            }

            return path;
        }
    }
}
