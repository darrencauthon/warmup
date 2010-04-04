using System;
using AppBus;
using StructureMap;

namespace warmup.Setup
{
    public class StructureMapMessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IContainer container;

        public StructureMapMessageHandlerFactory(IContainer container)
        {
            this.container = container;
        }

        public IMessageHandler<T> Create<T>(Type type)
        {
            return container.GetInstance(type) as IMessageHandler<T>;
        }
    }
}