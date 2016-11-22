using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

    internal sealed class NinjectFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IKernel kernel;

        public NinjectFilterAttributeFilterProvider(IKernel kernel)
        {
            this.kernel = kernel;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            foreach (var controllerAttribute in base.GetControllerAttributes(controllerContext, actionDescriptor))
            {
                kernel.Inject(controllerAttribute);
                yield return controllerAttribute;
            }
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            foreach (var actionAttribute in base.GetActionAttributes(controllerContext, actionDescriptor))
            {
                kernel.Inject(actionAttribute);
                yield return actionAttribute;
            }
        }
    }

    internal sealed class NinjectDataAnnotationsModelValidatorProvider : DataAnnotationsModelValidatorProvider
    {
        private readonly Func<DataAnnotationsModelValidator, ValidationAttribute> getter;
        private readonly IKernel kernel;

        public NinjectDataAnnotationsModelValidatorProvider(IKernel kernel)
        {
            var methodInfo = typeof(DataAnnotationsModelValidator).GetMethod("get_Attribute", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            this.getter = (Func<DataAnnotationsModelValidator, ValidationAttribute>)Delegate.CreateDelegate(typeof(Func<DataAnnotationsModelValidator, ValidationAttribute>), methodInfo, true);
            this.kernel = kernel;
        }

        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            foreach (var modelValidator in base.GetValidators(metadata, context, attributes))
            {
                var dataAnnotationsModelValidator = modelValidator as DataAnnotationsModelValidator;
                if (dataAnnotationsModelValidator != null)
                {
                    var validationAttribute = getter(dataAnnotationsModelValidator);
                    kernel.Inject(validationAttribute);
                }
                yield return modelValidator;
            }
        }
    }
}