using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _dutchContext;
        private readonly IWebHostEnvironment _env;

        public DutchSeeder(DutchContext dutchContext, IWebHostEnvironment env)
        {
            _dutchContext = dutchContext;
            _env = env;
        }

        public void Seed()
        {
            _dutchContext.Database.EnsureCreated();

            if (!_dutchContext.Products.Any())
            {
                //create sample data
                var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _dutchContext.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _dutchContext.Add(order);

                _dutchContext.SaveChanges();
            }
        }
    }
}
