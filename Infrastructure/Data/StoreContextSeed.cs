using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.products.Any())
            {
                var ProductsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products == null) return;

                context.products.AddRange(Products);

                await context.SaveChangesAsync();
            }
        }
    }
}
