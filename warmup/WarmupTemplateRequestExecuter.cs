﻿using System;
using System.Linq;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup
{
    public class WarmupTemplateRequestExecuter : IWarmupTemplateRequestExecuter
    {
        public void Execute(WarmupTemplateRequest warmupTemplateRequest)
        {
            RetrieveTheTemplateFiles(warmupTemplateRequest);

            ReplaceTokensInTheTemplateFiles(warmupTemplateRequest);
        }

        private static void ReplaceTokensInTheTemplateFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            Console.WriteLine("replacing tokens");
            (CreateTokenFileReplacer(warmupTemplateRequest)).ReplaceTokens(warmupTemplateRequest.TokenReplaceValue);
        }

        private static ITokensInFilesReplacer CreateTokenFileReplacer(WarmupTemplateRequest warmupTemplateRequest)
        {
            return new TokensInFilesReplacer(new PathDeterminer(warmupTemplateRequest.TokenReplaceValue));
        }

        private static IPathDeterminer CreatePathDeterminer(WarmupTemplateRequest warmupTemplateRequest)
        {
            return new PathDeterminer(warmupTemplateRequest.TokenReplaceValue);
        }

        private static void RetrieveTheTemplateFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            GetTemplateFileRetrievers(CreatePathDeterminer(warmupTemplateRequest))
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieve())
                                     retriever.RetrieveFiles(warmupTemplateRequest);
                             });
        }

        private static ITemplateFilesRetriever[] GetTemplateFileRetrievers(IPathDeterminer pathDeterminer)
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new ITemplateFilesRetriever[]{
                                                    new GitTemplateFilesRetriever(warmupConfigurationProvider, pathDeterminer),
                                                    new SvnTemplateFilesRetriever(warmupConfigurationProvider, pathDeterminer)
                                                };
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
        }
    }
}