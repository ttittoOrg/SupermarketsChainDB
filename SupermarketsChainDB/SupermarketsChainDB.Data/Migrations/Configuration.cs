namespace SupermarketsChainDB.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SupermarketsChainDB.Models;
    using System.Collections.Generic;

    public sealed class Configuration : DbMigrationsConfiguration<SupermarketSystemDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SupermarketSystemDbContext context)
        {
            //context.Database.ExecuteSqlCommand("ALTER TABLE VENDORS CREATE ");
            /*if (context.Measures.Any())
            {
                return;
            }

            var measures = new List<Measure> 
            {
                new Measure { MeasureName = "liters" },
                new Measure { MeasureName = "pieces" },
                new Measure { MeasureName = "kg" }
            };

            var vendors = new List<Vendor> 
            {
                new Vendor { VendorName  = "Nestle Sofia Corp." },
                new Vendor { VendorName  = "Zagorka Corp." },
                new Vendor { VendorName  = "Targovishte Bottling Company Ltd." }

            };

            var products = new List<Product>
            {
                new Product { ProductName = "Beer “Zagorka”", Price = 0.86M, VendorID = 20, MeasureID = 100, ProductType = ProductType.ÀlcoholDrinks },
                new Product { ProductName = "Vodka “Targovishte”", Price = 7.56M, VendorID = 30, MeasureID = 100, ProductType = ProductType.ÀlcoholDrinks },
                new Product { ProductName = "Beer “Beck’s”", Price = 1.03M, VendorID = 20, MeasureID = 100, ProductType = ProductType.ÀlcoholDrinks },
                new Product { ProductName = "Chocolate “Milka”", Price = 2.80M, VendorID = 10, MeasureID = 200, ProductType = ProductType.Confectionery },
            };

            foreach (var measure in measures)
            {
                context.Measures.Add(measure);
            }

            foreach (var vendor in vendors)
            {
                context.Vendors.Add(vendor);
            }

            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            context.SaveChanges();

            base.Seed(context);*/
        }
    }
}