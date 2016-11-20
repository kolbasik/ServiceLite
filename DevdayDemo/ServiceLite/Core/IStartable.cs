using System;
using JetBrains.Annotations;

namespace DevdayDemo.ServiceLite.Core
{
    public interface IStartable
    {
        void Start(StartContext context);
    }

    public interface IPreStartable
    {
        void PreStart(StartContext context);
    }

    public interface IPostStartable
    {
        void PostStart(StartContext context);
    }

    public sealed class StartContext
    {
        public StartContext([NotNull] IAppHost appHost)
        {
            if (appHost == null)
                throw new ArgumentNullException(nameof(appHost));
            AppHost = appHost;
        }

        public IAppHost AppHost { get; }
    }
}