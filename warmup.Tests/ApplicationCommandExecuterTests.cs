using Moq;
using NUnit.Framework;

namespace warmup.Tests
{
    [TestFixture]
    public class ApplicationCommandExecuterTests
    {
        [Test]
        public void Executes_first_command_that_can_be_executed()
        {
            var commandLineArguments = new string[]{};

            var executerFake = CreateCommandLineExecuterThatCanExecuteThis(commandLineArguments);

            var applicationCommandExecuter = new ApplicationCommandExecuter(new[]{executerFake.Object});

            applicationCommandExecuter.Execute(commandLineArguments);

            executerFake.Verify(x => x.Execute(commandLineArguments), Times.Once());
        }

        [Test]
        public void Does_not_execute_command_that_cannot_be_executed()
        {
            var commandLineArguments = new string[]{};

            var executerFake = CreateCommandLineExecuterThatCannotExecuteThis(commandLineArguments);

            var applicationCommandExecuter = new ApplicationCommandExecuter(new[]{executerFake.Object});

            applicationCommandExecuter.Execute(commandLineArguments);

            executerFake.Verify(x => x.Execute(commandLineArguments), Times.Never());
        }

        [Test]
        public void Only_executes_the_first_valid_executer()
        {
            var commandLineArguments = new string[]{};

            var firstExecuterFake = CreateCommandLineExecuterThatCanExecuteThis(commandLineArguments);
            var secondExecuterFake = CreateCommandLineExecuterThatCanExecuteThis(commandLineArguments);

            var applicationCommandExecuter = new ApplicationCommandExecuter(new[]{
                                                                                     firstExecuterFake.Object,
                                                                                     secondExecuterFake.Object
                                                                                 });

            applicationCommandExecuter.Execute(commandLineArguments);

            firstExecuterFake.Verify(x => x.Execute(commandLineArguments), Times.Once());
            secondExecuterFake.Verify(x => x.Execute(commandLineArguments), Times.Never());
        }

        private static Mock<ICommandLineCallExecuter> CreateCommandLineExecuterThatCanExecuteThis(string[] commandLineArguments)
        {
            var executerFake = new Mock<ICommandLineCallExecuter>();
            executerFake.Setup(x => x.CanExecute(commandLineArguments))
                .Returns(true);
            return executerFake;
        }

        private static Mock<ICommandLineCallExecuter> CreateCommandLineExecuterThatCannotExecuteThis(string[] commandLineArguments)
        {
            var executerFake = new Mock<ICommandLineCallExecuter>();
            executerFake.Setup(x => x.CanExecute(commandLineArguments))
                .Returns(false);
            return executerFake;
        }
    }
}