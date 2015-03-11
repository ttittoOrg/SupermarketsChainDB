namespace SupermarketsChainDB.Data
{
    using SupermarketsChainDB.Data.Repositories;
    using SupermarketsChainDB.Models;

    public interface ISupermarketSystemData
    {
        IGenericRepository<Product> Products { get; }

        IGenericRepository<Vendor> Vendors { get; }

        IGenericRepository<Measure> Measures { get; }

        int SaveChanges();
    }
}