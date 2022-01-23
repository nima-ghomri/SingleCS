using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SingleCS.Models
{
    /// <summary>
    /// C# Source file
    /// </summary>
    public class CSFile : ICSFile
    {
        public string Head { get; }
        public string Body { get; }


        public CSFile(string path)
        {
            var content = File.ReadAllText(path);
            var match = Regex.Match(content, @"((.|\n)*using [^(]*?;)((.|\n)*)$");
            Head = match.Groups[1].Value;
            Body = match.Groups[3].Value;
        }
    }
}
