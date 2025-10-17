using EFCore.BulkExtensions;
using BusX.Core.Enums;
using BusX.Data.Base;
using BusX.Data.Context;
using BusX.Data.Interfaces;
using BusX.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;
namespace BusX.Data.Helpers
{
    public class Repository<T>(BusXDbContext _db, IHttpContextAccessor _contextAccessor, bool _isAudit = false) : IRepository<T> where T : BaseEntity
    {
        private readonly BusXDbContext db = _db;
        private readonly IHttpContextAccessor contextAccessor = _contextAccessor;
        private DatabaseFacade Transaction { get; set; }
        public bool IsAudit = _isAudit;
        protected void Dispose() => db.Dispose();
        public void BulkHardDelete(List<T> entity) => db.BulkDelete(entity);
        public IQueryable<T> Where(Expression<Func<T, bool>> where) => db.Set<T>().Where(where);
        public void BeginTransaction()
        {
            Transaction = db.Database;
            if (Transaction.CurrentTransaction == null) Transaction.BeginTransaction();
        }
        public int UserID
        {
            get
            {
                if (contextAccessor != null &&
                contextAccessor.HttpContext != null &&
                contextAccessor.HttpContext.User != null &&
                contextAccessor.HttpContext.User.Claims.Any())
                {
                    var _userID = contextAccessor.HttpContext.User.Claims.FirstOrDefault(q => q.Type == "UID");
                    return _userID != null && !string.IsNullOrWhiteSpace(_userID.Value) ? _userID.Value.ToInt32() : (int)UserEnum.SYSTEM_USER;
                }
                return (int)UserEnum.SYSTEM_USER;
            }
        }
        public T Find(int ID)
        {
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return db.Set<T>().Find(ID);
        }
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] properties)
        {
            var query = db.Set<T>().AsNoTracking();
            if (properties.Length != 0) query = properties.Aggregate(query, (current, property) => current.Include(property));
            return query;
        }
        public void Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.CreateDate = DateTime.Now.ToUniversalTime();
            entity.CreateUserID = UserID;
            db.Add(entity);
            db.SaveChanges(IsAudit, UserID);
        }
        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.ModifyDate = DateTime.Now.ToUniversalTime();
            entity.ModifyUserID = UserID;
            db.Entry(entity).State = EntityState.Modified;
            db.Entry(entity).Property(x => x.CreateUserID).IsModified = false;
            db.Entry(entity).Property(x => x.CreateDate).IsModified = false;
            db.Entry(entity).Property(x => x.Guid).IsModified = false;
            db.SaveChanges(IsAudit, UserID);
        }
        public void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.IsDeleted = true;
            entity.DeleteDate = DateTime.Now.ToUniversalTime();
            entity.DeleteUserID = UserID;
            db.Entry(entity).State = EntityState.Modified;
            db.Entry(entity).Property(x => x.CreateUserID).IsModified = false;
            db.Entry(entity).Property(x => x.CreateDate).IsModified = false;
            db.Entry(entity).Property(x => x.ModifyUserID).IsModified = false;
            db.Entry(entity).Property(x => x.ModifyDate).IsModified = false;
            db.Entry(entity).Property(x => x.Guid).IsModified = false;
            db.SaveChanges(IsAudit, UserID);
        }
        public void HardDelete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            db.Remove(entity);
            db.SaveChanges(IsAudit, UserID);
        }
        public void BulkCreate(List<T> entity)
        {
            var createDate = DateTime.Now.ToUniversalTime();
            foreach (var item in entity)
            {
                item.CreateDate = createDate;
                item.CreateUserID = UserID;
            }
            db.BulkInsert(entity);
        }
        public void BulkUpdate(List<T> entity)
        {
            var updateDate = DateTime.Now.ToUniversalTime();
            foreach (var item in entity)
            {
                item.ModifyDate = updateDate;
                item.ModifyUserID = UserID;
            }
            var bulkConfig = new BulkConfig() { PropertiesToExclude = ["CreateDate", "CreateUserID", "GUID"] };
            db.BulkUpdate(entity);
        }
        public void BulkDelete(List<T> entity)
        {
            var deletedDate = DateTime.Now.ToUniversalTime();
            foreach (var item in entity)
            {
                item.DeleteDate = deletedDate;
                item.DeleteUserID = UserID;
                item.IsDeleted = true;
            }
            var bulkConfig = new BulkConfig() { PropertiesToExclude = ["CreateDate", "CreateUserID", "ModifyDate", "ModifyUserID", "GUID"] };
            db.BulkUpdate(entity);
        }
        ~Repository()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}