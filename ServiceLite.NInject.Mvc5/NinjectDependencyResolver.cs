using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;

namespace ServiceLite.NInject.Mvc5
{
    internal sealed class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public object GetService(Type serviceType) => kernel.TryGet(serviceType);
        public IEnumerable<object> GetServices(Type serviceType) => kernel.GetAll(serviceType).ToList();
    }
}