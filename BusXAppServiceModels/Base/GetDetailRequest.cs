#nullable enable
namespace BusX.Models.Base
{
    public class GetDetailRequest : GetDetailRequest<int> { }
    public class GetDetailRequestLong : GetDetailRequest<long> { }
    public class GetDetailRequest<T>
    {
        public T? ID { get; set; }
        public Guid? Guid { get; set; }
        public int? LanguageID { get; set; }
    }
}