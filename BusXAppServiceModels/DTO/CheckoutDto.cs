using BusXAppServiceModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.DTO
{
    public class CheckoutDto
    {
        public List<SeatInfo> SeatInfos { get; set; } = new List<SeatInfo>();
        public string CardNumber { get; set; } = string.Empty;
        public List<string> TcNo { get; set; } = new List<string>();
        public int JourneyID { get; set; }
    }
}
