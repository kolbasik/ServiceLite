using System;
using JetBrains.Annotations;

namespace ServiceLite.Core
{
    public static class ServiceProviderExtensions
    {
        public static TService GetService<TService>([NotNull] this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            return (TService) provider.GetService(typeof(TService));
        }

        public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            var service = provider.GetService(serviceType);
            if (service == null)
                throw new InvalidOperationException($@"Could not find the service of '{serviceType.FullName}' type.");
            return service;
        }

        public static T GetRequiredService<T>(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            return (T) provider.GetRequiredService(typeof(T));
        }
    }
}