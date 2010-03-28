using System;

namespace warmup.Bus
{
    public interface IMessageHandler
    {
        void Handle(object message);
        bool CanHandle(Type type, object message);
    }

    public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IEventMessage
    {
        void Handle(TMessage message);
    }

    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IEventMessage
    {
        public abstract void Handle(TMessage message);

        public virtual void Handle(object message)
        {
            Handle((TMessage)message);
        }

        public virtual bool CanHandle(Type type, object message)
        {
            return CanHandle(type, (TMessage)message);
        }

        public virtual bool CanHandle(Type type, TMessage message)
        {
            return type == typeof (TMessage) && CanHandle(message);
        }

        public virtual bool CanHandle(object message)
        {
            return CanHandle((TMessage)message);
        }

        public virtual bool CanHandle(TMessage message)
        {
            return true;
        }
    }
}