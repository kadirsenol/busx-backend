using AutoMapper;
using BusX.Data.Models;
using BusXAppServiceModels.DTO;
namespace BusX.GENAppService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Journey, JourneyDto>().ReverseMap();
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<InProcessJourneySeat, InProcessJourneySeatDto>().ReverseMap();
        }
    }
}