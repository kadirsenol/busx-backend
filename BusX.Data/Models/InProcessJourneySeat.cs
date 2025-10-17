using BusX.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusX.Data.Models
{
    public class InProcessJourneySeat : BaseEntity
    {
        public int InProcessJourneySeatID { get; set; }
        public int JourneyID { get; set; }
        public int SeatNo { get; set; }
    }
}
