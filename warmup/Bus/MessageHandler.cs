using System;

namespace warmup.Bus
{
    public interface IMessageHandler<TMessage>
    {
        Type Type { get; }
        bool CanHandle(TMessage message);
        void Handle(TMessage message);
    }

    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage>
    {
        public abstract void Handle(TMessage message);

        public Type Type
        {
            get { return typeof (TMessage); }
        }

        public virtual bool CanHandle(TMessage message)
        {
            return false;
        }

    }
}