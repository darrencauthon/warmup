using System;
using StructureMap;

namespace warmup.Bus
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler Create(Type type);
    }

    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IContainer container;

        public MessageHandlerFactory(IContainer container)
        {
            this.container = container;
        }

        public IMessageHandler Create(Type type)
        {
            return (IMessageHandler)container.GetInstance(type);
        }
    }
}