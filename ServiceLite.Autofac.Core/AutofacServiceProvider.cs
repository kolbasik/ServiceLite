using System;
using Autofac;

namespace ServiceLite.Autofac.Core
{
    public sealed class AutofacServiceProvider : IServiceProvider
    {
        public readonly IContainer Container;

        public AutofacServiceProvider(IContainer container)
        {
            this.Container = container;
        }

        public object GetService(Type serviceType) => this.Container.Resolve(serviceType);
    }
}