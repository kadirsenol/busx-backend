using BusX.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Request
{
    public class TicketSearch : SearchRequest
    {
        public string Pnr { get; set; } = string.Empty;
        public int? JourneyId { get; set; }
        public int? SeatNo { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerSurname { get; set; } = string.Empty;
        public bool? IsMale { get; set; }
        public decimal? TcNo { get; set; }
    }
}
