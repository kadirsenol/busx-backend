using BusX.Data.Base;
using System.Linq.Expressions;
namespace BusX.Data.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Find(int ID);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] properties);
        IQueryable<T> Where(Expression<Func<T, bool>> where);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void HardDelete(T entity);
        void BulkCreate(List<T> entity);
        void BulkUpdate(List<T> entity);
        void BulkDelete(List<T> entity);
        void BulkHardDelete(List<T> entity);
        int UserID { get; }
    }
}