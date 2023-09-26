using charity_website_backend.Entities;
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

        }
        private static IResult<IQueryable<SignUpRequestsDTO>> GetSignUpRequests( INGOService service, string search = "", int skip = 0, int take = 10)
        {
            return service.GetSignUpRequests();
        }
        private static IResult<bool> SendVerificationLink(INGOService service, int NGOId)
        {
            return service.SendVerificationLink(NGOId);
        }
        private static IResult<bool> VerifyNGO(INGOService service, int NGOId)
        {
            return service.VerifyNGO(NGOId);
        }

    }
}
