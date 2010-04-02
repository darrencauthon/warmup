using AutoMoq;
using NUnit.Framework;

namespace warmup.Tests
{
    [TestFixture]
    public class WarmupRequestMessageParserTests
    {
        private AutoMoqer mocker;

        [SetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();
        }

        [Test]
        public void GetArguments_Called_ReturnsCommandLineArgumentsClass()
        {
            var parser = mocker.Resolve<WarmupRequestMessageParser>();
            var arguments = parser.GetRequest(new string[]{});
            Assert.IsNotNull(arguments);
        }

        [Test]
        public void GetArguments_CalledWithTwoValues_IsValidIsTrue()
        {
            var parser = mocker.Resolve<WarmupRequestMessageParser>();
            var arguments = parser.GetRequest(new[]{"one", "two"});
            Assert.IsTrue(arguments.IsValid);
        }

        [Test]
        public void GetArguments_CalledWithOneValue_IsValidIsFalse()
        {
            var parser = mocker.Resolve<WarmupRequestMessageParser>();
            var arguments = parser.GetRequest(new[]{"one"});
            Assert.IsFalse(arguments.IsValid);
        }

        [Test]
        public void GetArguments_FirstArgumentIsBase_TemplateNameIsBase()
        {
            var parser = mocker.Resolve<WarmupRequestMessageParser>();
            var arguments = parser.GetRequest(new[]{"BASE"});
            Assert.AreEqual("BASE", arguments.TemplateName);
        }

        [Test]
        public void GetArguments_NoArgumentsPassed_IsValidIsFalse()
        {
            var parser = mocker.Resolve<WarmupRequestMessageParser>();
            var arguments = parser.GetRequest(new string[]{});
            Assert.IsFalse(arguments.IsValid);
        }

        [Test]
        public void GetArguments_SecondArgumentIsBase_TokenReplaceValueIsBase()
        {
            var parser = mocker.Resolve<WarmupRequestMessageParser>();
            var arguments = parser.GetRequest(new[]{"one", "BASE"});
            Assert.AreEqual("BASE", arguments.TokenReplaceValue);
        }
    }
}