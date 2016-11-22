using System.Linq;
using System.Web.Mvc;
using Ninject;
using ServiceLite.Core;

namespace ServiceLite.NInject.Mvc5
{
    public sealed class NInjectMvc5Feature : IPlugin, IConfigurable, IPostConfigurable
    {
        public NInjectMvc5Feature()
        {
            InjectFilterAttribute = true;
            InjectValidationAttribute = true;
            RegisterDependencyResolver = true;
        }

        public bool InjectFilterAttribute { get; set; }
        public bool InjectValidationAttribute { get; set; }
        public bool RegisterDependencyResolver { get; set; }

        public void Configure(ConfigurationContext context)
        {
            var kernel = context.AppHost.GetRequired<IKernel>();

            if (InjectFilterAttribute)
            {
                var providers = FilterProviders.Providers;
                foreach (var provider in providers.OfType<FilterAttributeFilterProvider>().ToArray())
                    providers.Remove(provider);
                providers.Add(new NinjectFilterAttributeFilterProvider(kernel));
            }
            if (InjectValidationAttribute)
            {
                var providers = ModelValidatorProviders.Providers;
                foreach (var provider in providers.OfType<DataAnnotationsModelValidatorProvider>().ToArray())
                    providers.Remove(provider);
                providers.Add(new NinjectDataAnnotationsModelValidatorProvider(kernel));
            }
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var kernel = context.AppHost.GetRequired<IKernel>();

            if (RegisterDependencyResolver)
                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        public void Start(StartContext context)
        {
        }
    }
}