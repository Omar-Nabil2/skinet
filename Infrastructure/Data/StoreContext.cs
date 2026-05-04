using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructure.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfigration).Assembly);
        }
    }
}
