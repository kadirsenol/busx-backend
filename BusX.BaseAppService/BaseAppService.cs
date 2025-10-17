using AutoMapper;
using BusX.AppServiceModels.Base;
using BusX.Data.Interfaces;
using BusX.Extensions;
using Microsoft.AspNetCore.Http;

namespace BusX.AppService.Base
{
    public class BaseAppService
    {
        private readonly IHttpContextAccessor contextAccessor;
        protected static DateTime CurrentServerDate => DateTime.Now;
        protected IMapper Mapper { get; set; }
        protected readonly IUnitOfWork UOW;
        public int LanguageID { get; set; }
        public BaseAppService(IUnitOfWork _uow, IMapper _mapper, IHttpContextAccessor _contextAccessor)
        {
            UOW = _uow;
            Mapper = _mapper;
            contextAccessor = _contextAccessor;
            if (LanguageID == 0 && User != null) LanguageID = User.LanguageID;
        }
        protected string GetClientIPAddress
        {
            get
            {
                var forwardedFor = contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                return string.IsNullOrWhiteSpace(forwardedFor) ? contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() : forwardedFor.Split(',')[0];
            }
        }
        protected UserJwtClaimDto User
        {
            get
            {
                var jwtClaimDto = new UserJwtClaimDto();
                if (contextAccessor != null &&
                    contextAccessor.HttpContext != null &&
                    contextAccessor.HttpContext.User != null &&
                    contextAccessor.HttpContext.User.Claims.Any())
                {
                    var userID = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtClaimDto.UID);
                    var userTypeID = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtClaimDto.UserTypeID);
                    var companyID = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtClaimDto.CompanyID);
                    var username = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtClaimDto.Username);
                    var isAuth = contextAccessor.HttpContext.User.Identity.IsAuthenticated;
                    // other properties will be added
                    if (userID != null && !string.IsNullOrWhiteSpace(userID.Value)) jwtClaimDto.UserID = userID.Value.ToInt32();
                    if (userTypeID != null && !string.IsNullOrWhiteSpace(userTypeID.Value)) jwtClaimDto.UserTypeID = userTypeID.Value.ToInt32();
                    if (companyID != null && !string.IsNullOrWhiteSpace(companyID.Value)) jwtClaimDto.CompanyID = companyID.Value.ToInt32();
                    if (username != null && !string.IsNullOrWhiteSpace(username.Value)) jwtClaimDto.Username = username.Value;
                    jwtClaimDto.IsAuth = isAuth;
                    jwtClaimDto.LanguageID = 1;
                    LanguageID = jwtClaimDto.LanguageID;
                }
                return jwtClaimDto;
            }
        }
    }
}