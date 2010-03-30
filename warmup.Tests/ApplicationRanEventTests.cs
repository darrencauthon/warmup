using System;
using System.Collections;
using System.Collections.Generic;
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
                EventMessage = eventMessage;
            }

            public void Add<T>(Type handlerType)
            {
                throw new NotImplementedException();
            }

            public void Send<T>(T eventMessage)
            {
                EventMessage = eventMessage;
            }

            public IEnumerator<Type> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(Type item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(Type item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Type[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(Type item)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public int IndexOf(Type item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, Type item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public Type this[int index]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public void Send(object message)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}