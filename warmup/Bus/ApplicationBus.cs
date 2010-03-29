using System;
using System.Collections.Generic;
using System.Linq;

namespace warmup.Bus
{
    public interface IApplicationBus
    {
        void Add<T>(Type handlerType);
        void Send<T>(T eventMessage);
    }

    public class ApplicationBus : List<Type>, IApplicationBus
    {
        private readonly IMessageHandlerFactory factory;
        private readonly IList<TypeRegistration> registeredTypes = new List<TypeRegistration>();

        public ApplicationBus(IMessageHandlerFactory factory)
        {
            this.factory = factory;
        }

        public void Add<T>(Type handlerType)
        {
            registeredTypes.Add(new TypeRegistration(handlerType, typeof (T)));
        }

        public void Send<T>(T eventMessage)
        {
            foreach (var handler in GetHandlersForType<T>())
                if (handler.CanHandle(eventMessage))
                    handler.Handle(eventMessage);
        }

        public IEnumerable<IMessageHandler<T>> GetHandlersForType<T>()
        {
            return from item in registeredTypes
                   where item.TypeOfEvent == typeof (T)
                   select factory.Create<T>(item.TypeOfHandler);
        }
    }

    public class TypeRegistration
    {
        public TypeRegistration(Type typeofHandler, Type typeofEvent)
        {
            TypeOfHandler = typeofHandler;
            TypeOfEvent = typeofEvent;
        }

        public Type TypeOfHandler { get; private set; }
        public Type TypeOfEvent { get; private set; }
    }
}