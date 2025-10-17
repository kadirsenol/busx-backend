using BusX.AppServiceModels.Request;
using BusX.AppServiceModels.Response;
using BusX.Models;
using BusX.Models.Base;
using BusXAppServiceModels.DTO;
using BusXAppServiceModels.Response;
namespace BusX.AppService.Interfaces
{
    public interface IJourneysAppService
    {
        ServiceResult<SearchResponse<JourneyListItem>> Search(JourneySearch request);
        ServiceResult<CreateOrEditResponse> CreateOrEdit(JourneyDto request);
        ServiceResult<DetailResponse<JourneyItem>> Info(GetDetailRequest request);
        ServiceResult<DetailResponse<JourneyDto>> Get(GetDetailRequest request);
        ServiceResult<bool> Delete(int ID);
        ServiceResult<SearchResponse<StationListItem>> GetAllStations();
        ServiceResult<SearchResponse<JourneySeatPlanListItem>> GetSeatsPlan(int id);
        ServiceResult<HealthCheckItem> CheckHealth();


        ServiceResult<CreateOrEditResponse> CreateOrEditJourneySeat(InProcessJourneySeatDto request);
        ServiceResult<bool> DeleteJourneySeat(int journeyID, int seatNo);

    }
}