using McMaster.Extensions.CommandLineUtils;
using SingleCS.Models;
using System.IO;
using System.Linq;

namespace SingleCS
{
    /// <summary>
    /// 
    /// </summary>
    /// SingleCS.exe <"files-path"> [-?|-h|--help] [-m|--main] [-r|--refactor] [-n|--n-merge] [-p|--p-merge]
    public class Program
    {
        public static CommandLineApplication Application { get; set; }

        private static string directory = Directory.GetCurrentDirectory();

        private static CommandArgument files;
        private static CommandOption exclude;
        private static CommandOption mains;
        private static CommandOption refactor;
        private static CommandOption nmerge;
        private static CommandOption pmerge;

        public static void Main(string[] args)
        {
            Application = new CommandLineApplication()
            {
                Name = "SingleCS",
                Description = "A command line tool to combine .cs files",
            };

            files = Application.Argument("files", "Files path", true);

            Application.HelpOption(inherited: true);
            exclude = Application.Option("-e|--exclude", "Exclude files", CommandOptionType.MultipleValue);
            mains = Application.Option("-m|--main", "Add main files", CommandOptionType.MultipleValue);
            mains = Application.Option("-d|--directory", "Specify working directory", CommandOptionType.MultipleValue);
            refactor = Application.Option("-r|--refactor", "Refactor usings and empty lines", CommandOptionType.NoValue);
            nmerge = Application.Option("-n|--n-merge", "Merge namespaces", CommandOptionType.NoValue);
            pmerge = Application.Option("-p|--p-merge", "Merge partial classes", CommandOptionType.NoValue);

            Application.OnExecute(OnExecute);

            Application.Execute(args);
        }

        private static void OnExecute()
        {
            var globber = new Globber(files.Values, exclude.Values, mains.Values, directory);
            var AllFiles = globber.AllFiles.ToDictionary(x => x, x => new CSFile(x));
            var FileSets = globber.FileSets.Select(x => x.Select(f => AllFiles[f]).ToArray());

            var combiner = new CSCombiner();
            foreach (var set in FileSets)
            {
                combiner.Combine(CombineOptions.None, set);
            }
        }
    }

}
