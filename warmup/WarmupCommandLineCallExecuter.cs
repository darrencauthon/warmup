using System;
using AppBus;

namespace warmup
{
    public class WarmupCommandLineCallExecuter : ICommandLineCallExecuter
    {
        private readonly IWarmupTemplateRequestParser warmupTemplateRequestParser;
        private readonly IApplicationBus applicationBus;

        public WarmupCommandLineCallExecuter(IWarmupTemplateRequestParser warmupTemplateRequestParser,
            IApplicationBus applicationBus)
        {
            this.warmupTemplateRequestParser = warmupTemplateRequestParser;
            this.applicationBus = applicationBus;
        }

        public bool CanExecute(string[] commandLineArguments)
        {
            return TheWarmupTemplateRequestIsValid(commandLineArguments);
        }

        public void Execute(string[] commandLineArguments)
        {
            applicationBus.Send(CreateWarmupTemplateRequest(commandLineArguments));
        }

        private bool TheWarmupTemplateRequestIsValid(string[] commandLineArguments)
        {
            return CreateWarmupTemplateRequest(commandLineArguments).IsValid;
        }

        private WarmupTemplateRequest CreateWarmupTemplateRequest(string[] commandLineArguments)
        {
            return warmupTemplateRequestParser.GetRequest(commandLineArguments);
        }


    }
}