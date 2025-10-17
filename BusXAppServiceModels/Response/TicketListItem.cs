using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Response
{
    public class TicketListItem
    {
        public int TicketID { get; set; }
        public string Pnr { get; set; } = string.Empty;
        public int JourneyId { get; set; }
        public int SeatNo { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerSurname { get; set; } = string.Empty;
        public bool IsMale { get; set; }
        public decimal TcNo { get; set; }
    }
}
