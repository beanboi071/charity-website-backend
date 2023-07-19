using charity_website_backend.Entities;

namespace charity_website_backend.Modules.Project.Services
{
    public interface IProjectService
    {
        IResult<EProject> Add(ProjectCreateDTO model,int NGOId);
    }
    public class ProjectCreateDTO
    {
        public string Title { get; set; }
        public decimal TargetAmount { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
    }
}
