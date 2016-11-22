using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ninject;

namespace ServiceLite.NInject.WebApi2
{
    internal sealed class NInjectDefaultFilterProvider : IFilterProvider
    {
        private readonly IKernel kernel;
        private readonly IEnumerable<IFilterProvider> filterProviders;

        public NInjectDefaultFilterProvider(IKernel kernel, params IFilterProvider[] filterProviders)
        {
            this.kernel = kernel;
            this.filterProviders = filterProviders;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            foreach (var filterInfo in this.filterProviders.SelectMany(fp => fp.GetFilters(configuration, actionDescriptor)))
            {
                this.kernel.Inject(filterInfo.Instance);
                yield return filterInfo;
            }
        }
    }
}