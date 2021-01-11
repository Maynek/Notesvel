//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library
{
    public delegate string ExpandHandler(string key);

    public static class Expander
    {
        public static ExpandHandler Handler { get; set; } = null;

        private static readonly Regex VariablesRegex = new Regex(
            @"\$\((?<VAR>.+?)\)",
            RegexOptions.Singleline | RegexOptions.Compiled
        );

        public static string Expand(string value)
        {
            if (Expander.Handler == null)
            {
                return value;
            }

            return Expander.VariablesRegex.Replace(value, delegate (Match match)
            {
                return Expander.Handler(match.Groups["VAR"].Value);
            });
        }
    }
}
