using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using Ninject;

namespace ServiceLite.NInject.Mvc5
{
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