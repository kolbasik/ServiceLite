using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;
using Ninject;

namespace ServiceLite.NInject.WebApi2
{
    internal sealed class NInjectDefaultModelValidatorProvider : ModelValidatorProvider
    {
        private readonly IKernel kernel;
        private readonly IEnumerable<ModelValidatorProvider> defaultModelValidatorProviders;

        public NInjectDefaultModelValidatorProvider(IKernel kernel, params ModelValidatorProvider[] defaultModelValidatorProviders)
        {
            this.kernel = kernel;
            this.defaultModelValidatorProviders = defaultModelValidatorProviders;
        }

        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            foreach (var modelValidator in defaultModelValidatorProviders.SelectMany(vp => vp.GetValidators(metadata, validatorProviders)))
            {
                this.kernel.Inject(modelValidator);
                yield return modelValidator;
            }
        }
    }
}