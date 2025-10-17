using BusXAppServiceModels.DTO;
using BusXAppServiceModels.Request;
using BusXAppServiceModels.Response;
using BusX.Models;
using BusX.Models.Base;
namespace BusX.AppService.Interfaces
{
    public interface ITicketsAppService
    {
        ServiceResult<SearchResponse<TicketListItem>> Search(TicketSearch request);
        ServiceResult<CreateOrEditResponse> CreateOrEdit(TicketDto request);
        ServiceResult<DetailResponse<TicketItem>> Info(GetDetailRequest request);
        ServiceResult<DetailResponse<TicketDto>> Get(GetDetailRequest request);
        ServiceResult<bool> Delete(int ID);
        ServiceResult<bool> Checkout(CheckoutDto request);
        ServiceResult<HealthCheckItem> CheckHealth();
    }
}