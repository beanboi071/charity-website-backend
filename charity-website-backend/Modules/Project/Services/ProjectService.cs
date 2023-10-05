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

        public IResult<ProjectDetailDTO> GetProjectDetails(int projectid)
        {
            var data = _context.Projects.Find(projectid);
            var projectDetails = new ProjectDetailDTO()
            {
                Id = data.Id,
                Title = data.Title,
                ImagePath = data.Image_Path,
                NGOId = data.NGO_Id,
                Description = data.Description,
                AmountRaised = data.Amount_Raised,
                CreatedDateTime = data.Created_Date_And_Time.ToString(),
                Status = data.Status,
                TargetAmount = data.Target_Amount,
            };
            return new IResult<ProjectDetailDTO>()
            {
                Data = projectDetails,
                Status = status.Success,
                Message = "Project details fetched sccessfully"
            };
        }
        public IResult<bool> ApproveProject(int ProjectId)
        {
            var project = _context.Projects.Find(ProjectId);
            project.Status = ProjectStatus.Approved;
            _context.SaveChanges();
            return new IResult<bool>()
            {
                Data = true,
                Status = status.Success,
            };
        }
        public  IResult<IQueryable<ProjectListDTO>> GetApprovedProjects()
        {
            var projects = _context.Projects.Where(x => x.Status == ProjectStatus.Approved).ToList();
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
        public IResult<IQueryable<PendingProjectListDTO>> GetPendingProjects()
        {
            var projects = _context.Projects.Where(x => x.Status == ProjectStatus.Pending).ToList();
            var data = (from c in projects
                        select new PendingProjectListDTO()
                        {
                            Id = c.Id,
                            ImagePath = c.Image_Path,
                            Title = c.Title,
                            NGOName = _context.NGOs.Find(c.NGO_Id).Name,
                            TargetAmount = c.Target_Amount
                        }).AsQueryable();
            return new IResult<IQueryable<PendingProjectListDTO>>()
            {
                Data = data,
                Status = status.Success
            };
        }
        public IResult<bool> DonateToProject(DonationDTO model, int donorId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var project = _context.Projects.Find(model.ProjectId);
                var donor = _context.Donors.Find(donorId);
                project.Amount_Raised += model.Amount;
                _context.SaveChanges();
                donor.Balance -= model.Amount;
                _context.SaveChanges();
                var donation = new EDonation()
                {
                    Donor_Id = donorId,
                    Amount = model.Amount,
                    Date_And_Time = DateTime.Now
                };
                _context.Donations.Add(donation);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new IResult<bool>() 
                {
                    Status = status.Failure,
                    Message = "Donation unsuccessful"
                };
            }
            
            return new IResult<bool>()
            {
                Data = true,
                Status = status.Success,
                Message = "Donation successful"
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
