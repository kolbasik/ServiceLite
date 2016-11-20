using System;
using System.Runtime;
using System.Web.Http;
using DevdayDemo.Services.Random;
using JetBrains.Annotations;

namespace DevdayDemo.Controllers
{
    [PublicAPI, RoutePrefix("api/v2/random")]
    public sealed class RandomController : ApiController
    {
        private readonly IRandomService randomService;

        public RandomController([NotNull] IRandomService randomService)
        {
            if (randomService == null)
                throw new ArgumentNullException(nameof(randomService));
            this.randomService = randomService;
        }

        [Route("")]
        public int Get([FromUri] int min = 0, [FromUri]int max = int.MaxValue)
        {
            return randomService.Generate(min, max);
        }
    }
}