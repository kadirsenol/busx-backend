using BusX.Data.Base.Interfaces;
using BusX.Data.Models;
namespace BusX.Data.Interfaces
{
    public interface IUnitOfWork : IUnitOfWorkBase
    {
        IRepository<Journey> JourneyRepository { get; }
        IRepository<Ticket> TicketRepository { get; }
        IRepository<Station> StationRepository { get; }
        IRepository<InProcessJourneySeat> InProcessJourneySeatRepository { get; }
    }
}