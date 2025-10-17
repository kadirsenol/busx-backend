namespace BusX.AppServiceModels.Base
{
    public class UserJwtClaimDto
    {
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int LanguageID { get; set; }
        public int UserTypeID { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsAuth { get; set; }
        public IList<int> UserGroupIDs { get; set; }
    }
}