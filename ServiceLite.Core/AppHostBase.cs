using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ServiceLite.Core
{
    public abstract class AppHostBase : IAppHost
    {
        public static readonly TraceSource Trace = new TraceSource(typeof(AppHostBase).FullName, SourceLevels.All);
        public static AppHostBase Instance;

        public AppHostBase()
        {
            Properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            Plugins = new List<IPlugin>();
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
            Instance = this;
        }

        public IDictionary<string, object> Properties { get; }
        public List<IPlugin> Plugins { get; }
        public IServiceProvider Container { get; private set; }
        private CancellationTokenSource CancellationTokenSource { get; }
        public CancellationToken CancellationToken { get; }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public virtual AppHostBase Configure(IServiceCollection container)
        {
            var context = new ConfigurationContext(this, container);
            Execute<IPreConfigurable>(this, x => x.PreConfigure(context));
            Execute<IPreConfigurable>(Plugins, x => x.PreConfigure(context));
            Execute<IConfigurable>(this, x => x.Configure(context));
            Execute<IConfigurable>(Plugins, x => x.Configure(context));
            ConfigureServices(container);
            Container = container.Build();
            Execute<IPostConfigurable>(Plugins, x => x.PostConfigure(context));
            Execute<IPostConfigurable>(this, x => x.PostConfigure(context));
            return this;
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public virtual AppHostBase Start()
        {
            if (Container == null)
                throw new NotSupportedException("Please ensure that .Configure(IServiceCollection container) method was called.");
            var context = new StartContext(this);
            Execute<IPreStartable>(this, x => x.PreStart(context));
            Execute<IPreStartable>(Plugins, x => x.PreStart(context));
            Execute<IStartable>(this, x => x.Start(context));
            Execute<IStartable>(Plugins, x => x.Start(context));
            Execute<IPostStartable>(Plugins, x => x.PostStart(context));
            Execute<IPostStartable>(this, x => x.PostStart(context));
            return this;
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        protected virtual void Execute<T>(object self, Action<T> execute) where T : class
        {
            try
            {
                var many = self as IEnumerable;
                if (many != null)
                {
                    foreach (var one in many)
                        Execute(one, execute);
                }
                else
                {
                    var desired = self as T;
                    if (desired != null)
                        execute(desired);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Trace.TraceData(TraceEventType.Critical, -1, ex);
                throw new ApplicationException("Could not start AppHost.", ex);
            }
        }

        public static void Release()
        {
            Release(Instance);
            Instance = null;
        }

        public static void Release(AppHostBase appHost)
        {
            appHost?.CancellationTokenSource.Cancel();
            Release(appHost as object);
            Release(appHost?.Container);
        }

        public static void Release(object disposable)
        {
            try
            {
                (disposable as IDisposable)?.Dispose();
            }
            catch (Exception ex)
            {
                Trace.TraceData(TraceEventType.Warning, -1, ex);
            }
        }
    }
}