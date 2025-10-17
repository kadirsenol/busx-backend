using BusX.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusX.Data.Models
{
    public class Station : BaseEntity
    {
        public int StationID { get; set; }
        public string City { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
