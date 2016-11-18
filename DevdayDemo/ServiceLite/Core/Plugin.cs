namespace DevdayDemo.ServiceLite.Core
{
    public abstract class Plugin
    {
        public virtual void Configure(IServiceCollection container)
        {
        }

        public virtual void Register(IAppHost appHost)
        {
        }
    }
}