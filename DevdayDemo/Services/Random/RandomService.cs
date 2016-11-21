using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;

namespace DevdayDemo.Services.Random
{
    [PublicAPI]
    public sealed class RandomService : IRandomService
    {
        private System.Random random;

        public RandomService()
        {
            random = new System.Random(Guid.NewGuid().GetHashCode());
        }

        public int Generate(int min, int max) => random.Next(min, max);
    }
}