namespace BusX.Models.Base
{
    public class SearchResponse<T>
    {
        public SearchResponse() { SearchResult = []; }
        public List<T> SearchResult { get; set; }
        public int TotalItemCount { get; set; }
    }
}