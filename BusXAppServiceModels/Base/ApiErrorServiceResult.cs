using BusX.Models.Enums;
namespace BusX.Models.API
{
    public class ApiErrorServiceResult<T>
    {
        public T ResultObject { get; set; }
        public string Message { get; set; }
        public MessageTypeEnum MessageType { get; set; } = MessageTypeEnum.None;
        public int StatusCode { get; set; }
        public ApiErrorServiceResult() { }
        public ApiErrorServiceResult(T result) => ResultObject = result;
        public ApiErrorServiceResult(MessageTypeEnum messageType, string message)
        {
            Message = message;
            MessageType = messageType;
        }
    }
}