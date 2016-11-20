using System;
using System.Web.Http;
using JetBrains.Annotations;

namespace DevdayDemo.Controllers
{
    [PublicAPI, RoutePrefix("api/v1/time")]
    public sealed class TimeController : ApiController
    {
        [Route("")]
        public DateTime Get() => DateTime.UtcNow;
    }
}