using charity_website_backend.Entities;

namespace charity_website_backend.Modules.Project.Services
{
    public interface IProjectService
    {
        IResult<EProject> Add(ProjectCreateDTO model,int NGOId);
        IResult<IQueryable<ProjectListDTO>> GetProjectsByNGOId(int NGOId);
    }
    public class ProjectCreateDTO
    {
        public string Title { get; set; }
        public decimal TargetAmount { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
    }
    public class ProjectListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImagePath { get; set; }
        public decimal AmountRaised { get; set; }
        public decimal TargetAmount { get; set; }
    }
}
