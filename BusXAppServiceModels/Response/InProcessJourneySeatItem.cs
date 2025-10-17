using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Response
{
    public class InProcessJourneySeatItem
    {
        public int InProcessJourneySeatID { get; set; }
        public int JourneyID { get; set; }
        public int SeatNo { get; set; }
    }
}
