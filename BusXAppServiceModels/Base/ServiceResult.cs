#nullable enable
using BusX.Models.Enums;
namespace BusX.Models
{
    public class ServiceResult<T>
    {
        public T? ResultObject { get; set; }
        public MessageTypeEnum MessageType { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public static ServiceResult<T> Success() => new() { IsSuccess = true, Message = string.Empty, MessageType = MessageTypeEnum.Success };
        public static ServiceResult<T> Success(string message) => new() { IsSuccess = true, Message = message, MessageType = MessageTypeEnum.Success };
        public static ServiceResult<T> Success(T result) => new() { ResultObject = result, IsSuccess = true, Message = string.Empty, MessageType = MessageTypeEnum.Success };
        public static ServiceResult<T> Success(T result, string message) => new() { ResultObject = result, IsSuccess = true, Message = message, MessageType = MessageTypeEnum.Success };
        public static ServiceResult<T> Error(T result, string message) => new() { ResultObject = result, IsSuccess = false, Message = message, MessageType = MessageTypeEnum.Error };
        public static ServiceResult<T> Error(string message) => new() { IsSuccess = false, Message = message, MessageType = MessageTypeEnum.Error };
    }
}