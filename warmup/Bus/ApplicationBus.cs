using System;
using System.Collections.Generic;

namespace warmup.Bus
{
    public class ApplicationBus : List<Type>, IApplicationBus
    {
        public new void Add(Type type)
        {
            if (type.GetInterface(typeof (IMessageHandler).Name) == null)
            {
                throw new InvalidOperationException(string.Format("Type {0} must implement the IMessageHandler interface", type.Name));
            }
            base.Add(type);
        }

        private IMessageHandlerFactory factory;

        public ApplicationBus(IMessageHandlerFactory factory)
        {
            this.factory = factory;
        }

        public void Send(IEventMessage eventMessage)
        {
            foreach (var handler in GetHandlersForType(eventMessage.GetType()))
            {
                handler.Handle(eventMessage);
            }
        }

// ReSharper disable ParameterHidesMember
        public void SetMessageHandlerFactory(IMessageHandlerFactory factory)
// ReSharper restore ParameterHidesMember
        {
            this.factory = factory;
        }

        public IEnumerable<IMessageHandler> GetHandlersForType(Type type)
        {
            foreach (var handlerType in this)
            {
                var handler = factory.Create(handlerType);
                if (handler.CanHandle(type))
                {
                    yield return handler;
                }
            }
        }
    }
}