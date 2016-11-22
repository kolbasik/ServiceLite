using System.Web.Http.Dependencies;
using Ninject;

namespace ServiceLite.NInject.WebApi2
{
    internal sealed class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
        }

        public IDependencyScope BeginScope() => this;
    }
}