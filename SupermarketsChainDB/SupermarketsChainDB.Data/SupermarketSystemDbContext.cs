namespace SupermarketsChainDB.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SupermarketsChainDB.Models;

    public class SupermarketSystemDbContext : DbContext, ISupermarketSystemDbContext
    {
        public SupermarketSystemDbContext()
            : base("SupermarketSystem")
        {
        }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Vendor> Vendors { get; set; }

        public IDbSet<Measure> Measures { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}