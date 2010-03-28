using System;
using AutoMoq;
using NUnit.Framework;

namespace warmup.Tests
{
    [TestFixture]
    public class ApplicationRanEventTests
    {
        private AutoMoqer mocker;

        [TestFixtureSetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();
        }

        [Test]
        public void Can_Handle_When_Warmup_Template_Request_Is_Valid()
        {
            var message = GetValidRequestMessage();

            var handler = mocker.Resolve<AttemptToExecuteWarmupMessageHandler>();

            var result = handler.CanHandle(message);
            Assert.IsTrue(result);
        }

        [Test]
        public void Cannot_Handle_When_Warmup_Template_Request_Is_Invalid()
        {
            var message = GetInvalidRequestMessage();

            var handler = mocker.Resolve<AttemptToExecuteWarmupMessageHandler>();

            var result = handler.CanHandle(message);
            Assert.IsFalse(result);
        }

        [Test]
        public void Handles_Valid_Request()
        {
            var request = new WarmupTemplateRequest{IsValid = true};

            var message = SetWarmupTemplateRequestParserToReturn(request);

            var executer = new ExecuterThatTracksTheRequestThatWasPassedToExecute();
            var handler = new AttemptToExecuteWarmupMessageHandler(mocker.GetMock<IWarmupTemplateRequestParser>().Object,
                                                                   executer);

            handler.Handle(message);
            Assert.AreSame(request, executer.RequestThatWasPassed);
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

        private class ExecuterThatTracksTheRequestThatWasPassedToExecute : IWarmupTemplateRequestExecuter
        {
            public WarmupTemplateRequest RequestThatWasPassed { get; set; }

            public void Execute(WarmupTemplateRequest warmupTemplateRequest)
            {
                if (RequestThatWasPassed != null)
                    throw new Exception("Should only be called once");
                RequestThatWasPassed = warmupTemplateRequest;
            }
        }
    }
}