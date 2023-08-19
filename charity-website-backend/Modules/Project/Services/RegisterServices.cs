
namespace charity_website_backend.Modules.Project.Services
{
    public static class RegisterServices
    {
        public static void RegisterProjectService(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IProjectService, ProjectService>();
        }
    }
}
