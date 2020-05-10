using System;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories
{
    internal abstract class HandlerBase
    {
        protected static THandler GetHandler<THandler>(ServiceProviderDelegate serviceProvider)
        {
            THandler handler;

            try
            {
                handler = serviceProvider.GetInstance<THandler>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error constructing handler for request of type {typeof(THandler)}. Register your handlers with the container.", e);
            }

            if (handler is null)
            {
                throw new InvalidOperationException($"Handler was not found for request of type {typeof(THandler)}. Register your handlers with the container.");
            }

            return handler;
        }
    }
}
