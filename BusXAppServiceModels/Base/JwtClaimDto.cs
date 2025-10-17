namespace BusX.AppServiceModels.Base
{
    public class JwtClaimDto
    {
        public static string UID { get { return "UID"; } }
        public static string APIKey { get { return "ApiKey"; } }
        public static string Username { get { return "Username"; } }
        public static string UserTypeID { get { return "UserTypeID"; } }
        public static string CompanyID { get { return "CompanyID"; } }
        public static string LanguageID { get { return "LanguageID"; } }
        public static string PermissonsWebAPI { get { return "PermissonsWebAPI"; } }
        public static string BackAPIModule { get { return "BackAPIModule"; } }
        public static string BackAPIController { get { return "BackAPIController"; } }
        public static string BackAPIAction { get { return "BackAPIAction"; } }
    }
}