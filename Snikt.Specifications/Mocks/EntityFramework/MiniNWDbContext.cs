using Snikt.Specifications.Mocks.Poco;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Snikt.Specifications.Mocks.EntityFramework
{
    internal class MiniNWDbContext : DbContext
    {
        public MiniNWDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            // HINT: Nothing to do here.
        }

        public DbSet<Category> Categories { get; set; }
    }
}
