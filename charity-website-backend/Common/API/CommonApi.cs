using charity_website_backend.Common.Services;
using charity_website_backend.Entities;
using charity_website_backend.Modules.LoginSignup.Services;

namespace charity_website_backend.Common.API
{
    public static class CommonApi
    {
        public static void RegisterCommonApi(this WebApplication app)
        {
            
            app.MapGet("File", GetFile);
            
        }
        private static Task GetFile(ICommonService service, string fileName, HttpContext context)
        {
            return service.GetImageFile(fileName, context);
        }
    }
}
