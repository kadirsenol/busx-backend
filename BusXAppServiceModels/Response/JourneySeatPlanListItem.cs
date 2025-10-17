using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Response
{
    public class SeatInfo
    {
        public int SeatNumber { get; set; }
        public bool IsMale { get; set; }
        public string PassengerSurname { get; set; } = string.Empty;
    }

    public class JourneySeatPlanListItem
    {
        public List<SeatInfo> FullSeats { get; set; } = new List<SeatInfo>();
        public List<int> EmptySeats { get; set; } = new List<int>();
    }
}
