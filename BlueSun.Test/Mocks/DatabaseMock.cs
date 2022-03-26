namespace BlueSun.Test.Mocks
{
    using BlueSun.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DatabaseMock
    {
        public static BlueSunDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<BlueSunDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new BlueSunDbContext(dbContextOptions);
            }
        }
    }
}
