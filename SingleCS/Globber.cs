using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingleCS
{
    public class Globber
    {
        public IEnumerable<IEnumerable<string>> FileSets { get; }
        public IEnumerable<string> AllFiles { get; }

        public Globber(IEnumerable<string> include, IEnumerable<string> exclude, IEnumerable<string> mains, string directory = null)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();

            var matcher = new Matcher();
            matcher.AddIncludePatterns(include);
            matcher.AddExcludePatterns(exclude);
            var filesPath = matcher.GetResultsInFullPath(directory);
            if (filesPath.Count() < 1)
                throw new InvalidOperationException("Found no valid matches.");

            matcher = new Matcher();
            matcher.AddIncludePatterns(mains);
            var mainsPath = matcher.GetResultsInFullPath(directory);
            if (mainsPath.Count() < 1)
                mainsPath = new[] { filesPath.First() };

            AllFiles = mainsPath.Union(filesPath);
            FileSets = mainsPath.Select(x => filesPath.Prepend(x).Distinct());
        }
    }

}
