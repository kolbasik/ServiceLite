using System;
using System.Collections.Generic;

namespace DevdayDemo.ServiceLite.Core
{
    public interface IAppHost
    {
        List<Plugin> Plugins { get; }
        IServiceProvider Container { get; }
    }
}