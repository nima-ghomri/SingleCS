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
        private IEnumerable<string> filesPath;


        public Globber(IEnumerable<string> include, IEnumerable<string> exclude, IEnumerable<string> mains, string directory)
        {
            filesPath = include.GroupBy(path => Path.IsPathFullyQualified(path) ? path : null).SelectMany((paths) =>
               {
                   var absolute = paths.Key != null;
                   var current = absolute ? Path.GetPathRoot(paths.First()) : directory;
                   var patterns = absolute ? new[] { Path.GetRelativePath(current, paths.First()) } : paths.ToArray();
                   var matcher = new Matcher();
                   matcher.AddIncludePatterns(patterns);
                   matcher.AddExcludePatterns(exclude);
                   return matcher.GetResultsInFullPath(current);
               });

            if (filesPath.Count() < 1)
                throw new InvalidOperationException("Not found any files.");

            var matcher = new Matcher();
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
