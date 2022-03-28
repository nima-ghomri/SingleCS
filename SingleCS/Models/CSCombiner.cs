using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SingleCS.Models
{
    public class CSCombiner : ICSCombiner
    {
        public ICSFile Combine(CombineOptions options, params ICSFile[] files)
        {
            var head = string.Join("", files.Select(f => f.Head));
            var body = string.Join("", files.Select(f => f.Body));

            if (options.HasFlag(CombineOptions.Refactor))
                Refactor(ref head, ref body);

            return new CSFile(head, body);
        }

        private void Refactor(ref string head, ref string body)
        {
            // Remove head comments
            head = Regex.Replace(head, @"\/\*(.|\n)*\*\/", string.Empty);
            head = string.Join("", head.Split(Environment.NewLine).Where(x => !x.StartsWith(@"//")));

            // Remove duplicates and format usings
            var matches = Regex.Matches(head, @"using[ |\n\r]+(\w*)([ |\n\r]*\.[ |\n\r]*(\w*))*[ |\n\r]*;");
            var usings = matches.Select(g => $"using {string.Join(".", g.Groups[3].Captures.Prepend(g.Groups[1]))};");
            head = string.Join(Environment.NewLine, usings.Distinct());
        }
    }
}
