using BusX.Data.Models;
using BusX.Data.Base;
using BusX.Data.Context;
using BusX.Data.Interfaces;
using Microsoft.AspNetCore.Http;
namespace BusX.Data.Helpers
{
    public class UnitOfWork(BusXDbContext db, IHttpContextAccessor contextAccessor) : UnitOfWorkBase(db, contextAccessor), IUnitOfWork
    {
        #region Private Repos
        private IRepository<Journey> _JourneyRepository;
        private IRepository<Ticket> _TicketRepository;
        private IRepository<Station> _StationRepository;
        private IRepository<InProcessJourneySeat> _InProcessJourneySeat;

        #endregion Private Repos

        #region Public Repos
        public IRepository<Journey> JourneyRepository => _JourneyRepository ??= new Repository<Journey>(db, contextAccessor, IsAudit);
        public IRepository<Ticket> TicketRepository => _TicketRepository ??= new Repository<Ticket>(db, contextAccessor, IsAudit);
        public IRepository<Station> StationRepository => _StationRepository ??= new Repository<Station>(db, contextAccessor, IsAudit);
        public IRepository<InProcessJourneySeat> InProcessJourneySeatRepository => _InProcessJourneySeat ??= new Repository<InProcessJourneySeat>(db, contextAccessor, IsAudit);

        #endregion Public Repos
    }
}