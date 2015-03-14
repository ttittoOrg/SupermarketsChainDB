namespace SupermarketsChainDB.Data
{
    using System;
    using System.Collections.Generic;
    using SupermarketsChainDB.Data.Repositories;
    using SupermarketsChainDB.Models;

    // Unit of work pattern
    public class SupermarketSystemData : ISupermarketSystemData
    {
        private ISupermarketSystemDbContext context;
        private IDictionary<Type, object> repositories;

        public SupermarketSystemData()
            : this(new SupermarketSystemDbContext())
        {
        }

        public SupermarketSystemData(ISupermarketSystemDbContext supermarketSystemDbContext)
        {
            this.context = supermarketSystemDbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<Product> Products
        {
            get
            {
                return this.GetRepository<Product>();
            }
        }

        public IGenericRepository<Vendor> Vendors
        {
            get
            {
                return this.GetRepository<Vendor>();
            }
        }

        public IGenericRepository<Measure> Measures
        {
            get
            {
                return this.GetRepository<Measure>();
            }
        }

        public IGenericRepository<Store> Stores
        {
            get
            {
                return this.GetRepository<Store>();
            }
        }

        public IGenericRepository<Sale> Sales
        {
            get
            {
                return this.GetRepository<Sale>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            Type typeOfModel = typeof(T);

            if (!this.repositories.ContainsKey(typeOfModel))
            {
                Type typeOfRepository = typeof(GenericRepository<T>);
                this.repositories.Add(typeOfModel, Activator.CreateInstance(typeOfRepository, this.context));
            }

            return this.repositories[typeOfModel] as IGenericRepository<T>;
        }
    }
}