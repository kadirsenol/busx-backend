using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.DTO
{
    public class JourneyDto
    {
        public int JourneyID { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Departure { get; set; }
        public string Provider { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int TotalSeat { get; set; }
        public bool IsWifi { get; set; } = false;
        public bool IsService { get; set; } = false;
        public bool IsTv { get; set; } = false;
        public bool IsAir { get; set; } = false;
    }
}
