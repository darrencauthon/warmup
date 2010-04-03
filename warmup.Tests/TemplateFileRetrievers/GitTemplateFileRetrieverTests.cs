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
    public class GitTemplateFileRetrieverTests
    {
        private AutoMoqer mocker;

        [TestFixtureSetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();

            // create the application bus here to get around automoqer issue
            mocker.GetMock<IApplicationBus>();

            SetupWarmupConfigurationForGit();
        }

        [Test]
        public void Can_retrieve_files_when_the_source_control_type_is_git()
        {
            SetupWarmupConfigurationForGit();

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

            var result = retriever.CanRetrieveTheFiles();
            Assert.IsTrue(result);
        }

        [Test]
        public void Cannot_retrieve_files_when_the_source_control_type_is_not_git()
        {
            SetupConfigurationForProviderThatIsNotGit();

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

            var result = retriever.CanRetrieveTheFiles();
            Assert.IsFalse(result);
        }

        [Test]
        public void Adds_git_retrieval_message_when_can_retrieve_git_files()
        {
            SetupWarmupConfigurationForGit();

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

            retriever.RetrieveTheFiles(new WarmupRequestMessage());
            mocker.GetMock<IApplicationBus>()
                .Verify(x => x.Send(It.IsAny<RetrieveFilesFromGitRepositoryMessage>()), Times.Once());
        }

        [Test]
        public void Sets_template_name_on_retrieve_files_message()
        {
            SetupWarmupConfigurationForGit();

            var templateName = string.Empty;

            mocker.GetMock<IApplicationBus>()
                .Setup(x => x.Send(It.IsAny<RetrieveFilesFromGitRepositoryMessage>()))
                .Callback((RetrieveFilesFromGitRepositoryMessage message) => { templateName = message.TemplateName; });

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

            retriever.RetrieveTheFiles(new WarmupRequestMessage{TemplateName = "template name"});

            Assert.AreEqual("template name", templateName);
        }

        [Test]
        public void Sets_token_replace_value_on_retrieve_files_message()
        {
            SetupWarmupConfigurationForGit();

            var tokenReplaceValue = string.Empty;

            mocker.GetMock<IApplicationBus>()
                .Setup(x => x.Send(It.IsAny<RetrieveFilesFromGitRepositoryMessage>()))
                .Callback((RetrieveFilesFromGitRepositoryMessage message) => { tokenReplaceValue = message.TokenReplaceValue; });

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

            retriever.RetrieveTheFiles(new WarmupRequestMessage{TokenReplaceValue = "token replace value"});

            Assert.AreEqual("token replace value", tokenReplaceValue);
        }

        [Test]
        public void Throws_exception_when_attempt_to_retrieve_files_when_source_control_type_is_not_git()
        {
            SetupConfigurationForProviderThatIsNotGit();

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

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
        public void Exception_returns_message_indicating_that_git_cannot_be_retrieved()
        {
            SetupConfigurationForProviderThatIsNotGit();

            var retriever = mocker.Resolve<GitTemplateFilesRetriever>();

            var message = string.Empty;
            try
            {
                retriever.RetrieveTheFiles(new WarmupRequestMessage());
            }
            catch (InvalidOperationException exception)
            {
                message = exception.Message;
            }

            Assert.AreEqual("System cannot retrieve the files using git", message);
        }

        private void SetupWarmupConfigurationForGit()
        {
            mocker.GetMock<IWarmupConfigurationProvider>()
                .Setup(x => x.GetWarmupConfiguration())
                .Returns(new WarmupConfiguration("location", "git"));
        }

        private void SetupConfigurationForProviderThatIsNotGit()
        {
            mocker.GetMock<IWarmupConfigurationProvider>()
                .Setup(x => x.GetWarmupConfiguration())
                .Returns(new WarmupConfiguration("location", "not git"));
        }
    }
}