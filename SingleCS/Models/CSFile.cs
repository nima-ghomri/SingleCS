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
        public string Path { get; }


        public CSFile(string path)
        {
            Path = path;
            var content = File.ReadAllText(path);
            var match = Regex.Match(content, @"^(.*using [^(]*?;)*(.*)$", RegexOptions.Singleline);
            Head = match.Groups[1].Value;
            Body = match.Groups[2].Value;
        }

        public CSFile(string head, string body)
        {
            Head = head;
            Body = body;
        }

        public override string ToString()
        {
            return $"{Head}{Body}";
        }
    }
}
