namespace BusX.Models.Base
{
    public class CreateOrEditResponse
    {
        public long ID { get; set; }
        public string Guid { get; set; } = string.Empty;
        public int AffectedRow { get; set; }
        public int FeatureTypeID { get; set; }
        public int LanguageID { get; set; }
        public string ExternalData { get; set; } = string.Empty;
    }
}