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
        public string Usings { get; }
        public string Body { get; }


        public CSFile(string path)
        {
            var content = File.ReadAllText(path);
            var match = Regex.Match(content, @"((.|\n)*using [^(]*?;)((.|\n)*)$");
            //Usings = Regex.Match(content, @"(.|\n)*^using [^(]*?;").Value;
            //Body = Regex.Match(content, @"name[^$]*$").Value;
        }
    }
}
