using System;
using System.Collections.Generic;

namespace DevdayDemo.ServiceLite.Core
{
    public interface IAppHost
    {
        IDictionary<string, object> Properties { get; }
        List<Plugin> Plugins { get; }
        IServiceProvider Container { get; }
    }
}