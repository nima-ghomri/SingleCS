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
            filesPath = FindAllRelativeAndAbsolute(include, exclude, directory);

            if (filesPath.Count() < 1)
                throw new InvalidOperationException("Not found any files.");

            mainsPath = FindAllRelativeAndAbsolute(mains, null, directory);
            if (mainsPath.Count() < 1)
                mainsPath = new[] { filesPath.First() };
        }

        private IEnumerable<string> FindAllRelativeAndAbsolute(IEnumerable<string> include, IEnumerable<string> exclude = null, string directory = null)
        {
            return include.GroupBy(path => Path.IsPathFullyQualified(path) ? path : null).SelectMany((paths) =>
             {
                 var absolute = paths.Key != null;
                 var current = absolute ? Path.GetPathRoot(paths.First()) : directory ?? Directory.GetCurrentDirectory();
                 var patterns = absolute ? new[] { Path.GetRelativePath(current, paths.First()) } : paths.ToArray();
                 var matcher = new Matcher();
                 matcher.AddIncludePatterns(patterns);
                 if (exclude != null)
                     matcher.AddExcludePatterns(exclude);
                 return matcher.GetResultsInFullPath(current);
             });
        }

        public IEnumerable<IEnumerable<ICSFile>> GetFileSets(Func<string, ICSFile> creator)
        {
            var files = filesPath.Select(x => creator(x));
            var mains = mainsPath.Select(x => creator(x));

            return mains.Select(x => files.Where(f => f.Path != x.Path).Prepend(x));
        }
    }

}
