using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);

        IEnumerable<T> GetAllFunc(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        T? Get(
            Expression<Func<T, bool>> filter,
            string? includeProperties = null);

        T? GetFunc(
            Expression<Func<T, bool>> filter, 
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        void Add(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
    }
}
