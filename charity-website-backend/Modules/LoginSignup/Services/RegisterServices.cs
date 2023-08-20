using charity_website_backend.Common.Services;

namespace charity_website_backend.Modules.LoginSignup.Services
{
    public static class RegisterServices
    {
        public static void RegisterLoginSignupService(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ILoginSignupService, LoginSignupService>();
            builder.Services.AddTransient<ICommonService, CommonService>();
        }
    }
}
