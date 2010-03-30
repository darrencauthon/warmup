namespace warmup
{
    public class ApplicationCommandExecuter
    {
        private readonly ICommandLineCallExecuter[] commandLineCallExecuters;

        public ApplicationCommandExecuter(ICommandLineCallExecuter[] commandLineCallExecuters)
        {
            this.commandLineCallExecuters = commandLineCallExecuters;
        }

        public void Execute(string[] commandLineArguments)
        {
            foreach (var executer in commandLineCallExecuters)
                if (executer.CanExecute(commandLineArguments))
                {
                    executer.Execute(commandLineArguments);
                    return;
                }
        }
    }
}