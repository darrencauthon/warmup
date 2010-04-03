using System.IO;
using AppBus;
using warmup.Messages;

namespace warmup.Behaviors
{
    public class DetermineThePathToPutTheFiles : IMessageHandler<GetTargetFilePathMessage>
    {
        public void Handle(GetTargetFilePathMessage message)
        {
            var path = Path.GetFullPath(message.TokenReplaceValue);
            message.Result = new GetTargetFilePathResult{Path = path};
        }
    }
}