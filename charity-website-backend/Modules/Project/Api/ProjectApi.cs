using charity_website_backend.Common.Services;
using charity_website_backend.Entities;
using charity_website_backend.Modules.Project.Services;

namespace charity_website_backend.Modules.Project.Api
{
    public static class ProjectApi
    {
        public static void RegisterProjectApi(this WebApplication app)
        {
            var root = "Api/ProjectApi/";
            app.MapPost(root + "CreateProject", Create);
        }
        private static IResult<EProject> Create(ProjectCreateDTO model, IProjectService service, ISessionService sessionService)
        {
            var session = sessionService.GetSession();
            int NGOId = session.Id;
            return service.Add(model, NGOId);
        }
    }
}
