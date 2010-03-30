using System;

namespace warmup
{
    public interface ICommandLineCallExecuter
    {
        bool CanExecute(string[] commandLineArguments);
        void Execute(string[] commandLineArguments);
    }
}