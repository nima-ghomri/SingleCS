using McMaster.Extensions.CommandLineUtils;
using SingleCS.Models;
using System;
using System.Collections.Generic;
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

        private static CommandArgument files;
        private static CommandOption exclude;
        private static CommandOption mains;
        private static CommandOption directory;
        private static CommandOption template;

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

            directory = Application.Option("-d|--directory", "Specify working directory", CommandOptionType.SingleValue);
            directory.DefaultValue = Directory.GetCurrentDirectory();

            template = Application.Option("-o|--output", "Output files template, ex. '* - Merged.cs'", CommandOptionType.SingleValue);
            template.DefaultValue = "* - Merged.cs";

            refactor = Application.Option("-r|--refactor", "Refactor usings and empty lines", CommandOptionType.NoValue);
            nmerge = Application.Option("-n|--n-merge", "Merge namespaces", CommandOptionType.NoValue);
            pmerge = Application.Option("-p|--p-merge", "Merge partial classes", CommandOptionType.NoValue);

            Application.OnExecute(OnExecute);

            try
            {
                Application.Execute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void OnExecute()
        {
            CombineOptions options = CombineOptions.None;
            if (refactor.HasValue())
                options |= CombineOptions.Refactor;
            if (nmerge.HasValue())
                options |= CombineOptions.NameSpaceMerge;
            if (pmerge.HasValue())
                options |= CombineOptions.PartialMerge;


            var globber = new Globber(files.Values, exclude.Values, mains.Values, directory.Value());
            var sets = globber.GetFileSets(x => new CSFile(x));

            var pathTemplate = template.Value().Replace("*", "{0}", StringComparison.InvariantCultureIgnoreCase);

            var combiner = new CSCombiner();
            foreach (var set in sets)
            {
                var file = combiner.Combine(options, set.ToArray());
                var path = string.Format(pathTemplate, Path.GetFileNameWithoutExtension(set.First().Path));
                File.WriteAllText(path, file.ToString());
            }
        }

    }
}
