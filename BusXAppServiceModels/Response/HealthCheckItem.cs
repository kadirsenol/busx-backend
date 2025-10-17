using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusXAppServiceModels.Response
{
    public class HealthCheckItem
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool DatabaseConnected { get; set; }
        public string ServerTime { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
    }
}
