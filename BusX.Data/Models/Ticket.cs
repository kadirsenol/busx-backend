using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusX.Data.Base;

namespace BusX.Data.Models
{
    public class Ticket : BaseEntity
    {
        public int TicketID { get; set; }
        public string Pnr { get; set; } = string.Empty;
        public int JourneyId { get; set; }
        public int SeatNo { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerSurname { get; set; } = string.Empty;
        public bool IsMale { get; set; }
        public decimal TcNo { get; set; }

        [ForeignKey(nameof(JourneyId))]
        public virtual Journey? Journey { get; set; }
    }
}
