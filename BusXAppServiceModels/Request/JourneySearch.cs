using BusX.Models.Base;
namespace BusX.AppServiceModels.Request
{
    public class JourneySearch : SearchRequest
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public TimeSpan? Departure { get; set; }
        public string Provider { get; set; } = string.Empty;
        public decimal? BasePrice { get; set; }
        public int? TotalSeat { get; set; }
        public bool IsWifi { get; set; } = false;
        public bool IsService { get; set; } = false;
        public bool IsTv { get; set; } = false;
        public bool IsAir { get; set; } = false;
    }
}
