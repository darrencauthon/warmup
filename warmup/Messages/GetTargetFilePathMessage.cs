using AppBus;

namespace warmup.Messages
{
    public class GetTargetFilePathMessage : IQueryMessage<GetTargetFilePathResult>
    {
        public string TokenReplaceValue { get; set; }
        public GetTargetFilePathResult Result { get; set; }
    }

    public class GetTargetFilePathResult
    {
        public string Path { get; set; }
    }
}