using charity_website_backend.Common.Services;
using charity_website_backend.Modules.LoginSignup.Services;

namespace charity_website_backend.Modules.NGO.Services
{
    public static class RegisterServices
    {
        public static void RegisterNGOService(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<INGOService, NGOService>();
        }
    }
}
