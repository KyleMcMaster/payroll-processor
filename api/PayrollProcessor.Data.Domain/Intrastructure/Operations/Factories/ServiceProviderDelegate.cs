using System;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Factories
{
    public delegate object ServiceProviderDelegate(Type serviceType);

    public static class ServiceFactoryExtensions
    {
        public static T GetInstance<T>(this ServiceProviderDelegate factory)
            => (T)factory(typeof(T));
    }
}
