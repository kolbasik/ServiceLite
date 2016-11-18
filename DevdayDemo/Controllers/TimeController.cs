using System;
using System.Web.Http;

namespace DevdayDemo.Controllers
{
    [RoutePrefix("api/v1/time")]
    public sealed class TimeController : ApiController
    {
        [Route("")]
        public DateTime Get() => DateTime.UtcNow;
    }
}