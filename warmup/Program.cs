﻿using System;
using System.Linq;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arguments = GetCommandLineArguments(args);

            var targetDir = PullDownTheTemplateFilesIntoDirectory(arguments);

            ReplaceTokensInTheTemplate(arguments, targetDir);
        }

        private static void ReplaceTokensInTheTemplate(CommandLineArgumentSet arguments, TargetDir targetDir)
        {
            Console.WriteLine("replacing tokens");
            targetDir.ReplaceTokens(arguments.TokenReplaceValue);
        }

        private static TargetDir PullDownTheTemplateFilesIntoDirectory(CommandLineArgumentSet arguments)
        {
            var baseUri = new Uri(GetTheWarmupConfigurationProvider().GetWarmupConfiguration().SourceControlWarmupLocation + arguments.TemplateName);
            var targetDir = new TargetDir(arguments.TokenReplaceValue);

            GetTemplateFileRetrievers()
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieve())
                                     retriever.RetrieveFiles(baseUri, targetDir);
                             });
            return targetDir;
        }

        private static ITemplateFilesRetriever[] GetTemplateFileRetrievers()
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new ITemplateFilesRetriever[]{
                                                    new GitTemplateFilesRetriever(warmupConfigurationProvider),
                                                    new SvnTemplateFilesRetriever(warmupConfigurationProvider)
                                                };
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
        }

        private static CommandLineArgumentSet GetCommandLineArguments(string[] args)
        {
            var parser = new CommandLineArgumentParser();
            var arguments = parser.GetArguments(args);
            if (arguments.IsValid == false)
                throw new ArgumentException("Command line arguments are not valid");
            return arguments;
        }
    }
}