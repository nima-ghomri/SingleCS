using McMaster.Extensions.CommandLineUtils;
using System;
using System.Linq;

namespace SingleCS
{
    /// <summary>
    /// 
    /// </summary>
    /// SingleCS.exe <"files-path"> [-?|-h|--help] [-m|--main] [-r|--refactor] [-n|--n-merge] [-p|--p-merge]
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = "SingleCS",
                Description = "A command line tool to combine .cs files",
            };

            var files = app.Argument("files", "Files path", true);

            app.HelpOption(inherited: true);
            var mains = app.Option("-m|--main", "Add main files", CommandOptionType.MultipleValue);
            var refactor = app.Option("-r|--refactor", "Refactor usings and empty lines", CommandOptionType.NoValue);
            var nmerge = app.Option("-n|--n-merge", "Merge namespaces", CommandOptionType.NoValue);
            var pmerge = app.Option("-p|--p-merge", "Merge partial classes", CommandOptionType.NoValue);

            app.Execute(args);
        }

    }
}
