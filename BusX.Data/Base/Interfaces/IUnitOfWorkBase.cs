namespace BusX.Data.Base.Interfaces
{
    public interface IUnitOfWorkBase
    {
        void BeginTransaction();
        void CommitTransaction();
        void DisposeTransaction();
        void CommitAndDisposeTransaction();
        void RollBack();
        void StartAuditLog();
    }
}