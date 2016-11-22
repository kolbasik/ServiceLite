using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;

namespace ServiceLite.NInject.WebApi2
{
    internal class NinjectDependencyScope : IDependencyScope
    {
        public IKernel Kernel { get; }

        public NinjectDependencyScope(IKernel kernel)
        {
            this.Kernel = kernel;
        }

        public object GetService(Type serviceType) => Kernel.TryGet(serviceType);
        public IEnumerable<object> GetServices(Type serviceType) => Kernel.GetAll(serviceType);

        public void Dispose()
        {
        }
    }
}