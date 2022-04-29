using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingleCS.Models
{
    public class Globber
    {
        private IEnumerable<string> mainsPath;
        private List<string> filesPath;


        public Globber(IEnumerable<string> include, IEnumerable<string> exclude, IEnumerable<string> mains, string directory)
        {
            var paths = include.GroupBy(p => Path.IsPathFullyQualified(p)).ToDictionary(p => p.Key);
            var relatives = paths[false];
            var absolutes = paths[true];

            var matcher = new Matcher();
            matcher.AddIncludePatterns(relatives);
            matcher.AddExcludePatterns(exclude);
            filesPath = new List<string>();
            filesPath.AddRange(matcher.GetResultsInFullPath(directory));

            foreach (var path in absolutes)
            {
                matcher = new Matcher();
                var pathDirectory = Path.GetDirectoryName(path);
                var pattern = Path.GetRelativePath(pathDirectory, path);
                matcher.AddInclude(pattern);
                filesPath.AddRange(matcher.GetResultsInFullPath(pathDirectory));
            }

            if (filesPath.Count() < 1)
                throw new InvalidOperationException("Not found any files.");

            matcher = new Matcher();
            matcher.AddIncludePatterns(mains);
            mainsPath = matcher.GetResultsInFullPath(directory);
            if (mainsPath.Count() < 1)
                mainsPath = new[] { filesPath.First() };
        }

        public IEnumerable<IEnumerable<ICSFile>> GetFileSets(Func<string, ICSFile> creator)
        {
            var files = filesPath.Select(x => creator(x));
            var mains = mainsPath.Select(x => creator(x));

            return mains.Select(x => files.Where(f => f.Path != x.Path).Prepend(x));
        }
    }

}
