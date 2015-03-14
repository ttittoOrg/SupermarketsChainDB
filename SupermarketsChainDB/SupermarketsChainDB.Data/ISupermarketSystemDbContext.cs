namespace SupermarketsChainDB.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using SupermarketsChainDB.Models;

    public interface ISupermarketSystemDbContext
    {
        IDbSet<Product> Products { get; set; }

        IDbSet<Vendor> Vendors { get; set; }

        IDbSet<Measure> Measures { get; set; }

        IDbSet<Store> Stores { get; set; }

        IDbSet<Sale> Sales { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}