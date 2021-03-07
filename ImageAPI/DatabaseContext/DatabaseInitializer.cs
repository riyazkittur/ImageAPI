using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.DatabaseContext
{
    public static class DatabaseInitializer
    {
        public static void Initialize(ImageDbContext context)
        {
            context.Database.EnsureCreated();

        }
    }
}
