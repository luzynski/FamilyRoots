using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine;
using FamilyRoots.Cli.Options;

namespace FamilyRoots.Cli
{
    static class Program
    {
        static void Main(string[] args)
        {  
            var types = ;
            Parser.Default.ParseArguments(args, LoadVerbs())
                .WithParsed(Run)
                .WithNotParsed(HandleErrors);
        }

        private	static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray(); 
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            //Do nothing, print default parser message.
        }

        private static void Run(object obj)
        {
            switch (obj)
            {
                case CloneOptions c:
                    //process CloneOptions
                    break;
                case CommitOptions o:
                    //process CommitOptions
                    break;
                case AddOptions a:
                    //process AddOptions
                    break;
            }
        }

}