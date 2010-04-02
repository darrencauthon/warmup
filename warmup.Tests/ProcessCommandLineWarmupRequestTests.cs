using System;
using AppBus;
using AutoMoq;
using NUnit.Framework;
using warmup.Messages;

namespace warmup.Tests
{
    [TestFixture]
    public class ProcessCommandLineWarmupRequestTests
    {
        private AutoMoqer mocker;

        [TestFixtureSetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();
        }

        [Test]
        public void Handles_Valid_Request()
        {
            var request = new WarmupRequestMessage{IsValid = true};

            var message = SetWarmupTemplateRequestParserToReturn(request);

            var testBus = new TestBus();
            var handler = new ProcessCommandLineWarmupRequest(mocker.GetMock<IWarmupRequestMessageParser>().Object, testBus);

            handler.Handle(message);
            Assert.AreSame(request, testBus.EventMessage);
        }

        private CommandLineMessage SetWarmupTemplateRequestParserToReturn(WarmupRequestMessage requestMessage)
        {
            var message = new CommandLineMessage{CommandLineArguments = new string[]{}};
            mocker.GetMock<IWarmupRequestMessageParser>()
                .Setup(x => x.GetRequest(message.CommandLineArguments))
                .Returns(requestMessage);
            return message;
        }

        private CommandLineMessage GetValidRequestMessage()
        {
            var message = CreateApplicationRanMessage();
            TheRequestIsValid(message);
            return message;
        }

        private CommandLineMessage GetInvalidRequestMessage()
        {
            var message = CreateApplicationRanMessage();
            TheRequestIsInvalid(message);
            return message;
        }

        private void TheRequestIsInvalid(CommandLineMessage message)
        {
            mocker.GetMock<IWarmupRequestMessageParser>()
                .Setup(x => x.GetRequest(message.CommandLineArguments))
                .Returns(new WarmupRequestMessage{IsValid = false});
        }

        private void TheRequestIsValid(CommandLineMessage message)
        {
            mocker.GetMock<IWarmupRequestMessageParser>()
                .Setup(x => x.GetRequest(message.CommandLineArguments))
                .Returns(new WarmupRequestMessage{IsValid = true});
        }

        private static CommandLineMessage CreateApplicationRanMessage()
        {
            return new CommandLineMessage{CommandLineArguments = new string[]{}};
        }

        #region test bus

        private class TestBus : IApplicationBus
        {
            public object EventMessage { get; private set; }

            public void Send(IEventMessage eventMessage)
            {
            }

            public void Send<T>(T message)
            {
                EventMessage = message;
            }

            public void Add(Type handlerType)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}