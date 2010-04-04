using System.IO;
using AutoMoq;
using NUnit.Framework;
using warmup.Behaviors;
using warmup.Messages;

namespace warmup.Tests.Behaviors
{
    [TestFixture]
    public class DetermineThePathToPutTheFilesTests
    {
        private AutoMoqer mocker;

        [TestFixtureSetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();
        }

        [Test]
        public void Put_the_files_at_the_full_path_of_the_token_replace_value()
        {
            var expectedPath = Path.GetFullPath("test");

            var message = new GetTargetFilePathMessage{TokenReplaceValue = "test"};
            var determiner = mocker.Resolve<DetermineThePathToPutTheFilesBehavior>();
            determiner.Handle(message);

            Assert.AreEqual(expectedPath, message.Result.Path);
        }
    }
}