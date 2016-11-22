using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using Ninject;
using ServiceLite.Core;

namespace ServiceLite.NInject.WebApi2
{
    public sealed class NInjectWebApiFeature : IPlugin, IConfigurable, IPostConfigurable
    {
        public NInjectWebApiFeature()
        {
            InjectFilters = true;
            InjectModelValidators = true;
            RegisterDependencyResolver = true;
        }

        public bool InjectFilters { get; set; }
        public bool InjectModelValidators { get; set; }
        public bool RegisterDependencyResolver { get; set; }

        public void Configure(ConfigurationContext context)
        {
            var kernal = context.AppHost.GetRequired<IKernel>();
            var config = context.AppHost.GetRequired<HttpConfiguration>();

            if (InjectFilters)
            {
                var defaultFilterProviders = config.Services.GetServices(typeof(IFilterProvider)).OfType<IFilterProvider>().ToArray();
                config.Services.Clear(typeof(IFilterProvider));
                config.Services.Add(typeof(IFilterProvider), new NInjectDefaultFilterProvider(kernal, defaultFilterProviders));
            }
            if (InjectModelValidators)
            {
                var defaultModelValidatorProviders = config.Services.GetServices(typeof(ModelValidatorProvider)).OfType<ModelValidatorProvider>().ToArray();
                config.Services.Clear(typeof(ModelValidatorProvider));
                config.Services.Add(typeof(ModelValidatorProvider), new NInjectDefaultModelValidatorProvider(kernal, defaultModelValidatorProviders));
            }
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var kernal = context.AppHost.GetRequired<IKernel>();
            var config = context.AppHost.GetRequired<HttpConfiguration>();

            if (RegisterDependencyResolver)
                config.DependencyResolver = new NinjectDependencyResolver(kernal);
        }

        public void Start(StartContext context)
        {
        }
    }
}