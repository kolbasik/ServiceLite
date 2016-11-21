using System;
using System.Diagnostics;
using Autofac;
using JetBrains.Annotations;

namespace ServiceLite.Autofac.Core
{
    public sealed class AutofacServiceProvider : IServiceProvider
    {
        public readonly IContainer Container;

        public AutofacServiceProvider([NotNull] IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            this.Container = container;
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public object GetService(Type serviceType) => this.Container.Resolve(serviceType);
    }
}