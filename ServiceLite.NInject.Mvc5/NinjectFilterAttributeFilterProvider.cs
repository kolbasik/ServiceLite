using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;

namespace ServiceLite.NInject.Mvc5
{
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
}