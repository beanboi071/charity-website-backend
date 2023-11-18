using charity_website_backend.Common.Services;
using charity_website_backend.Entities;
using charity_website_backend.Modules.Donor.Services;
using charity_website_backend.Modules.NGO.Services;

namespace charity_website_backend.Modules.Donor.Api
{
    public static class DonorApi
    {
        public static void RegisterDonorApi(this WebApplication app)
        {
            var root = "Api/DonorApi/";
            app.MapGet(root + "MyProfile", GetUserProfile);
            app.MapPut(root + "MyProfile", UpdateUserProfile);

        }
        private static IResult<DonorProfileVM> GetUserProfile(IDonorService service, ISessionService sessionService)
        {
            var session = sessionService.GetSession();
            int UserId = session.Id;
            return service.GetUserProfile(UserId);
        }
        private static IResult<int> UpdateUserProfile(IDonorService service, ISessionService sessionService, DonorProfileVM model)
        {
            var session = sessionService.GetSession();
            int UserId = session.Id;
            return service.UpdateUserProfile(model,UserId);
        }
    }
}
