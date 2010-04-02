using System;
using AppBus;
using AutoMoq;
using Moq;
using NUnit.Framework;

namespace warmup.Tests
{
    [TestFixture]
    public class WarmupCommandLineCallExecuterTests
    {
        private AutoMoqer mocker;

        [TestFixtureSetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();
        }

        [Test]
        public void Can_execute_when_the_parser_says_the_request_is_valid()
        {
            var commandLineArguments = new string[]{};
            SetupParserThatCanHandleThese(commandLineArguments);

            var executer = mocker.Resolve<WarmupCommandLineCallExecuter>();

            var result = executer.CanExecute(commandLineArguments);
            Assert.IsTrue(result);
        }

        [Test]
        public void Cannot_execute_when_the_parser_says_the_request_is_invalid()
        {
            var commandLineArguments = new string[]{};
            SetupParserThatCannotHandleThese(commandLineArguments);

            var executer = mocker.Resolve<WarmupCommandLineCallExecuter>();

            var result = executer.CanExecute(commandLineArguments);
            Assert.IsFalse(result);
        }

        [Test]
        public void Execute_puts_warmup_template_request_on_bus()
        {
            var commandLineArguments = new string[]{};

            SetupParserThatCanHandleThese(commandLineArguments);

            var bus = new TestBus();
            var executer = new WarmupCommandLineCallExecuter(mocker.GetMock<IWarmupTemplateRequestParser>().Object, bus);

            executer.Execute(commandLineArguments);
            Assert.IsNotNull(bus.EventMessage);
        }

        [Test]
        public void Execute_passes_the_request_on_the_message_on_the_bus()
        {
            var expectedRequest = new WarmupTemplateRequest();
            var commandLineArguments = new string[] { };

            var parserFake = CreateParserFakeThatWillReturnThis(commandLineArguments, expectedRequest);

            var bus = new TestBus();

            var executer = new WarmupCommandLineCallExecuter(parserFake.Object, bus);

            executer.Execute(commandLineArguments);
            Assert.AreSame(expectedRequest, bus.EventMessage);
        }

        private Mock<IWarmupTemplateRequestParser> CreateParserFakeThatWillReturnThis(string[] commandLineArguments, WarmupTemplateRequest expectedRequest)
        {
            var parserFake = mocker.GetMock<IWarmupTemplateRequestParser>();
            parserFake.Setup(x => x.GetArguments(commandLineArguments))
                .Returns(expectedRequest);
            return parserFake;
        }

        public class TestBus : IApplicationBus
        {
            public object EventMessage { get; set; }

            public void Send<T>(T message)
            {
                EventMessage = message;
            }

            public void Add(Type messageHandlerType)
            {
                throw new NotImplementedException();
            }
        }

        private void SetupParserThatCanHandleThese(string[] commandLineArguments)
        {
            mocker.GetMock<IWarmupTemplateRequestParser>()
                .Setup(x => x.GetArguments(commandLineArguments))
                .Returns(new WarmupTemplateRequest{IsValid = true});
        }

        private void SetupParserThatCannotHandleThese(string[] commandLineArguments)
        {
            mocker.GetMock<IWarmupTemplateRequestParser>()
                .Setup(x => x.GetArguments(commandLineArguments))
                .Returns(new WarmupTemplateRequest{IsValid = false});
        }
    }
}