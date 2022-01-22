using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SingleCS.Models
{
    public class CSFile
    {
        public List<string> Usings { get; }
        public string Body { get; }


        public CSFile(string path)
        {
            var content = File.ReadAllText(path);
            Usings = Regex.Matches(content, @"using(.*);").Select(g => g.Value).ToList();
            Body = Regex.Match(content, @"name[^$]*$").Value;
        }
    }
}
