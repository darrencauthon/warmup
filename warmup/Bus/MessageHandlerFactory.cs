using System;
using StructureMap;

namespace warmup.Bus
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler<T> Create<T>(Type type);
    }

    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IContainer container;

        public MessageHandlerFactory(IContainer container)
        {
            this.container = container;
        }

        public IMessageHandler<T> Create<T>(Type type)
        {
            return (IMessageHandler<T>)container.GetInstance(type);
        }
    }
}