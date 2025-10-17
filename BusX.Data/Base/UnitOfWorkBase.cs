using BusX.Data.Base.Interfaces;
using BusX.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
namespace BusX.Data.Base
{
    public class UnitOfWorkBase(BusXDbContext _db, IHttpContextAccessor _contextAccessor) : IUnitOfWorkBase
    {
        private bool Disposed = false;
        public bool IsAudit = false;
        protected readonly BusXDbContext db = _db;
        protected readonly IHttpContextAccessor contextAccessor = _contextAccessor;
        private IDbContextTransaction transaction;
        public void StartAuditLog() => IsAudit = true;
        public void RollBack() { if (db.Database.CurrentTransaction == null) db.Database.RollbackTransaction(); }
        public void BeginTransaction() { if (db.Database.CurrentTransaction == null) transaction = db.Database.BeginTransaction(); }
        public void CommitTransaction() { if (db.Database.CurrentTransaction != null) transaction.Commit(); }
        public void DisposeTransaction() { if (db.Database.CurrentTransaction != null) transaction.Dispose(); }
        public void CommitAndDisposeTransaction()
        {
            if (db.Database.CurrentTransaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                transaction?.Dispose();
                db.Dispose();
            }
            Disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}