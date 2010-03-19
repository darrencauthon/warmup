using System;

namespace warmup
{
    public interface ISourceControlTemplateHandler
    {
        void Export(Uri sourceLocation, TargetDir target);
    }
}