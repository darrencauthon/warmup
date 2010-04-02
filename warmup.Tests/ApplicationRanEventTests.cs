using System;
using AppBus;
using AutoMoq;
using NUnit.Framework;

namespace warmup.Tests
{
    [TestFixture]
    public class WarmupRequestFromCommandLineHandlerTests
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
            var request = new WarmupTemplateRequest{IsValid = true};

            var message = SetWarmupTemplateRequestParserToReturn(request);

            var testBus = new TestBus();
            var handler = new WarmupRequestFromCommandLineHandler(mocker.GetMock<IWarmupTemplateRequestParser>().Object, testBus);

            handler.Handle(message);
            Assert.AreSame(request, testBus.EventMessage);
        }

        private ApplicationRanMessage SetWarmupTemplateRequestParserToReturn(WarmupTemplateRequest request)
        {
            var message = new ApplicationRanMessage{CommandLineArguments = new string[]{}};
            mocker.GetMock<IWarmupTemplateRequestParser>()
                .Setup(x => x.GetArguments(message.CommandLineArguments))
                .Returns(request);
            return message;
        }

        private ApplicationRanMessage GetValidRequestMessage()
        {
            var message = CreateApplicationRanMessage();
            TheRequestIsValid(message);
            return message;
        }

        private ApplicationRanMessage GetInvalidRequestMessage()
        {
            var message = CreateApplicationRanMessage();
            TheRequestIsInvalid(message);
            return message;
        }

        private void TheRequestIsInvalid(ApplicationRanMessage message)
        {
            mocker.GetMock<IWarmupTemplateRequestParser>()
                .Setup(x => x.GetArguments(message.CommandLineArguments))
                .Returns(new WarmupTemplateRequest{IsValid = false});
        }

        private void TheRequestIsValid(ApplicationRanMessage message)
        {
            mocker.GetMock<IWarmupTemplateRequestParser>()
                .Setup(x => x.GetArguments(message.CommandLineArguments))
                .Returns(new WarmupTemplateRequest{IsValid = true});
        }

        private static ApplicationRanMessage CreateApplicationRanMessage()
        {
            return new ApplicationRanMessage{CommandLineArguments = new string[]{}};
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