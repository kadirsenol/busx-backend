using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Response
{
    public class StationItem
    {
        public int StationID { get; set; }
        public string City { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
