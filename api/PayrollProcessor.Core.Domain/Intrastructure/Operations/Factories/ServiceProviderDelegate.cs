using System;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;

public delegate object ServiceProviderDelegate(Type serviceType);

public static class ServiceFactoryExtensions
{
    public static T GetInstance<T>(this ServiceProviderDelegate serviceProvider)
        => (T)serviceProvider(typeof(T));
}
