using System;
using System.Collections.Generic;

namespace warmup.Bus
{
    public interface IApplicationBus : IList<Type>
    {
        void Send(IEventMessage eventMessage);
        void SetMessageHandlerFactory(IMessageHandlerFactory factory);
    }
}