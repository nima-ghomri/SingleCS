using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
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
            mains = Application.Option("-m|--main", "Add main files", CommandOptionType.MultipleValue);
            refactor = Application.Option("-r|--refactor", "Refactor usings and empty lines", CommandOptionType.NoValue);
            nmerge = Application.Option("-n|--n-merge", "Merge namespaces", CommandOptionType.NoValue);
            pmerge = Application.Option("-p|--p-merge", "Merge partial classes", CommandOptionType.NoValue);

            Application.OnExecute(OnExecute);

            Application.Execute(args);
        }

        private static void OnExecute()
        {
        }
    }
}
