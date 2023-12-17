using charity_website_backend.Common.Model;
using charity_website_backend.Common.Services;
using charity_website_backend.Entities;
using charity_website_backend.Modules.Donor.Services;
using charity_website_backend.Modules.LoginSignup.Services;
using charity_website_backend.Modules.NGO.Services;

namespace charity_website_backend.Modules.NGO.Api
{
    public static class NGOApi
    {
        public static void RegisterNGOApi(this WebApplication app)
        {
            var root = "Api/NGOApi/";
            app.MapGet(root + "GetSignUpRequests", GetSignUpRequests);
            app.MapGet(root + "SendVerificationLink", SendVerificationLink);
            app.MapGet(root + "VerifyNGO", VerifyNGO);
            app.MapGet(root + "MyProfile", GetUserProfile);
            app.MapPut(root + "MyProfile", UpdateUserProfile);
            app.MapGet(root + "List", List);
            app.MapGet(root + "GetNGODetail", GetNGODetail);
        }
        private static IResult<IQueryable<SignUpRequestsDTO>> GetSignUpRequests( INGOService service, string search = "", int skip = 0, int take = 10)
        {
            return service.GetSignUpRequests();
        }
        private static IResult<ListVM<NGOListVM>> List( INGOService service, string search = "", int skip = 0, int take = 10)
        {
            return service.List(search.ToLower(),skip,take);
        }
        private static IResult<bool> SendVerificationLink(INGOService service, int NGOId)
        {
            return service.SendVerificationLink(NGOId);
        }
        private static IResult<bool> VerifyNGO(INGOService service, int NGOId)
        {
            return service.VerifyNGO(NGOId);
        }
        private static IResult<NGOProfileVM> GetUserProfile(INGOService service, ISessionService sessionService)
        {
            int UserId = sessionService.Id;
            int userType = sessionService.UserType;
            return service.GetUserProfile(UserId);
        }
        private static IResult<NGOListVM> GetNGODetail(INGOService service, int ngoId)
        {
            return service.GetNGODetail(ngoId);
        }
        private static IResult<int> UpdateUserProfile(INGOService service, ISessionService sessionService, NGOProfileVM model)
        {
            int UserId = sessionService.Id;
            return service.UpdateUserProfile(model, UserId);
        }
    }
}
