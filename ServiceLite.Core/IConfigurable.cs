using System;
using JetBrains.Annotations;

namespace ServiceLite.Core
{
    public interface IConfigurable
    {
        void Configure(ConfigurationContext context);
    }

    public interface IPreConfigurable
    {
        void PreConfigure(ConfigurationContext context);
    }

    public interface IPostConfigurable
    {
        void PostConfigure(ConfigurationContext context);
    }

    public sealed class ConfigurationContext
    {
        public ConfigurationContext([NotNull] IAppHost appHost, [NotNull] IServiceCollection serviceCollection)
        {
            if (appHost == null)
                throw new ArgumentNullException(nameof(appHost));
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));
            AppHost = appHost;
            ServiceCollection = serviceCollection;
        }

        public IAppHost AppHost { get; }
        public IServiceCollection ServiceCollection { get; }
    }
}