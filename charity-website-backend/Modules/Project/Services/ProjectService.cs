using charity_website_backend.Common.Services;
using charity_website_backend.DB;
using charity_website_backend.Entities;

namespace charity_website_backend.Modules.Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly CharityDbContext _context;
        public ProjectService(CharityDbContext context)
        {
            _context = context;
        }

        public IResult<EProject> Add(ProjectCreateDTO model, int NGOId)
        {
            var projectData = new EProject()
            {
                Title = model.Title,
                Description = model.Description,
                Target_Amount = model.TargetAmount,
                Created_Date_And_Time = DateTime.Now,
                Status = ProjectStatus.Pending,
                Amount_Raised = 0,
                NGO_Id = NGOId,
                Image_Path = null
            };
            
            if (model.ImageBase64.StartsWith("data:image/"))
            {
                var fileName = model.ImageBase64.ToFileUrl("Project",  Guid.NewGuid().ToString());
                projectData.Image_Path = fileName;
                
            }
            _context.Projects.Add(projectData);
            _context.SaveChanges();
            return new IResult<EProject>()
            {
                Status = status.Success
            };
        }

        public IResult<IQueryable<ProjectListDTO>> GetProjectsByNGOId(int NGOId)
        {
            var projects = _context.Projects.Where(x => x.NGO_Id == NGOId).ToList();
            var data = (from c in projects
                        select new ProjectListDTO()
                        {
                            Id = c.Id,
                            ImagePath = c.Image_Path,
                            Title = c.Title,
                            AmountRaised = c.Amount_Raised,
                            TargetAmount = c.Target_Amount
                        }).AsQueryable();
            return new IResult<IQueryable<ProjectListDTO>>()
            {
                Data = data,
                Status = status.Success
            };
        }
    }
}
