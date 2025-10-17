using BusX.Data.Interfaces;
namespace BusX.Data.Base
{
    public abstract class BaseEntity : ISoftDelete
    {
        public int? CreateUserID { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int? ModifyUserID { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? DeleteUserID { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}