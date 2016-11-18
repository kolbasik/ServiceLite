using System;
using System.Web.Http;
using DevdayDemo.Services.Random;

namespace DevdayDemo.Controllers
{
    [RoutePrefix("api/v1/random")]
    public sealed class RandomController : ApiController
    {
        private readonly IRandomService randomService;

        public RandomController(IRandomService randomService)
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