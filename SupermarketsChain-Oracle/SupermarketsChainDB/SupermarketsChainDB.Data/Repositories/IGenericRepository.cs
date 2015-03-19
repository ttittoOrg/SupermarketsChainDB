namespace SupermarketsChainDB.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    // Repository pattern
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> All();

        IQueryable<T> Search(Expression<Func<T, bool>> conditions);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Detach(T entity);

        int SaveChanges();
    }
}