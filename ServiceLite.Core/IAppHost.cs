using System;
using System.Collections.Generic;

namespace ServiceLite.Core
{
    public interface IAppHost
    {
        IDictionary<string, object> Properties { get; }
        List<IPlugin> Plugins { get; }
        IServiceProvider Container { get; }
    }
}