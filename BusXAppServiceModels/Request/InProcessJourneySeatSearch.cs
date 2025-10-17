using BusX.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Request
{
    public class InProcessJourneySeatSearch : SearchRequest
    {
        public int JourneyID { get; set; }
        public int SeatNo { get; set; }
    }
}
