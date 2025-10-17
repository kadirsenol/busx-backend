using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.DTO
{
    public class InProcessJourneySeatDto
    {
        public int InProcessJourneySeatID { get; set; }
        public int JourneyID { get; set; }
        public int SeatNo { get; set; }
    }
}
