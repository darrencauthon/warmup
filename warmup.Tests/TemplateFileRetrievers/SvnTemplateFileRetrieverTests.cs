using System;
using AppBus;
using AutoMoq;
using Moq;
using NUnit.Framework;
using warmup.Messages;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup.Tests.TemplateFileRetrievers
{
    [TestFixture]
    public class SvnTemplateFileRetrieverTests
    {
        private AutoMoqer mocker;

        [TestFixtureSetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();

            // create the application bus here to get around automoqer issue
            mocker.GetMock<IApplicationBus>();

            SetupWarmupConfigurationForSvn();
        }

        [Test]
        public void Can_retrieve_files_when_the_source_control_type_is_Svn()
        {
            SetupWarmupConfigurationForSvn();

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            var result = retriever.CanRetrieveTheFiles();
            Assert.IsTrue(result);
        }

        [Test]
        public void Cannot_retrieve_files_when_the_source_control_type_is_not_Svn()
        {
            SetupConfigurationForProviderThatIsNotSvn();

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            var result = retriever.CanRetrieveTheFiles();
            Assert.IsFalse(result);
        }

        [Test]
        public void Adds_Svn_retrieval_message_when_can_retrieve_Svn_files()
        {
            SetupWarmupConfigurationForSvn();

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            retriever.RetrieveTheFiles(new WarmupRequestMessage());
            mocker.GetMock<IApplicationBus>()
                .Verify(x => x.Send(It.IsAny<RetrieveFilesFromSvnRepositoryMessage>()), Times.Once());
        }

        [Test]
        public void Sets_template_name_on_retrieve_files_message()
        {
            SetupWarmupConfigurationForSvn();

            var templateName = string.Empty;

            mocker.GetMock<IApplicationBus>()
                .Setup(x => x.Send(It.IsAny<RetrieveFilesFromSvnRepositoryMessage>()))
                .Callback((RetrieveFilesFromSvnRepositoryMessage message) => { templateName = message.TemplateName; });

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            retriever.RetrieveTheFiles(new WarmupRequestMessage{TemplateName = "template name"});

            Assert.AreEqual("template name", templateName);
        }

        [Test]
        public void Sets_token_replace_value_on_retrieve_files_message()
        {
            SetupWarmupConfigurationForSvn();

            var tokenReplaceValue = string.Empty;

            mocker.GetMock<IApplicationBus>()
                .Setup(x => x.Send(It.IsAny<RetrieveFilesFromSvnRepositoryMessage>()))
                .Callback((RetrieveFilesFromSvnRepositoryMessage message) => { tokenReplaceValue = message.TokenReplaceValue; });

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            retriever.RetrieveTheFiles(new WarmupRequestMessage{TokenReplaceValue = "token replace value"});

            Assert.AreEqual("token replace value", tokenReplaceValue);
        }

        [Test]
        public void Throws_exception_when_attempt_to_retrieve_files_when_source_control_type_is_not_Svn()
        {
            SetupConfigurationForProviderThatIsNotSvn();

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            var exceptionThrown = false;
            try
            {
                retriever.RetrieveTheFiles(new WarmupRequestMessage());
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void Exception_returns_message_indicating_that_Svn_cannot_be_retrieved()
        {
            SetupConfigurationForProviderThatIsNotSvn();

            var retriever = mocker.Resolve<SvnTemplateFilesRetriever>();

            var message = string.Empty;
            try
            {
                retriever.RetrieveTheFiles(new WarmupRequestMessage());
            }
            catch (InvalidOperationException exception)
            {
                message = exception.Message;
            }

            Assert.AreEqual("System cannot retrieve the files using svn", message);
        }

        private void SetupWarmupConfigurationForSvn()
        {
            mocker.GetMock<IWarmupConfigurationProvider>()
                .Setup(x => x.GetWarmupConfiguration())
                .Returns(new WarmupConfiguration("location", "Svn"));
        }

        private void SetupConfigurationForProviderThatIsNotSvn()
        {
            mocker.GetMock<IWarmupConfigurationProvider>()
                .Setup(x => x.GetWarmupConfiguration())
                .Returns(new WarmupConfiguration("location", "not Svn"));
        }
    }
}