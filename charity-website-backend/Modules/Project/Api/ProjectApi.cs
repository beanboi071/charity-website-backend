using charity_website_backend.Common.Model;
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
            app.MapGet(root + "GetProjectsByNGOId", GetProjectsByNGOId);
            app.MapGet(root + "GetApprovedProjects", GetApprovedProjects);
            app.MapGet(root + "GetPendingProjects", GetPendingProjects);
            app.MapGet(root + "GetProjectDetails", GetProjectDetails);
            app.MapPost(root + "DonateToProject", DonateToProject);
            app.MapGet(root + "ApproveProject", ApproveProject);
        }
        private static IResult<bool> DonateToProject(DonationDTO model, IProjectService service, ISessionService sessionService)
        {
            var session = sessionService.GetSession();
            int DonorId = session.Id;
            return service.DonateToProject(model, DonorId);
        }
        private static IResult<EProject> Create(ProjectCreateDTO model, IProjectService service, ISessionService sessionService)
        {
            var session = sessionService.GetSession();
            int NGOId = session.Id;
            return service.Add(model, NGOId);
        }
        private static IResult<ListVM<ProjectListDTO>> GetApprovedProjects(IProjectService service, string search = "",string ngoName = "", int skip = 0,int take = 10)
        {
            return service.GetApprovedProjects(search.ToLower(),ngoName.ToLower(),skip,take);
        }
        private static IResult<bool> ApproveProject(IProjectService service, int projectId)
        {
            return service.ApproveProject(projectId);
        }
        private static IResult<ListVM<PendingProjectListDTO>> GetPendingProjects(IProjectService service, string search = "",string ngoName = "", int skip = 0, int take = 10)
        {
            return service.GetPendingProjects(search.ToLower(),ngoName.ToLower(), skip, take);
        }
        private static IResult<ListVM<ProjectListDTO>> GetProjectsByNGOId(IProjectService service, ISessionService sessionService, string search = "", int skip = 0, int take = 10)
        {
            var session = sessionService.GetSession();
            int NGOId = session.Id;
            return service.GetProjectsByNGOId(NGOId, search.ToLower(), skip, take);

        }
        private static IResult<ProjectDetailDTO> GetProjectDetails(IProjectService service, int projectId)
        {
            return service.GetProjectDetails(projectId);
        }
    }
}
