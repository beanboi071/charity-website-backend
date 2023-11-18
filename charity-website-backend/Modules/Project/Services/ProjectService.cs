﻿using charity_website_backend.Common.Model;
using charity_website_backend.Common.Services;
using charity_website_backend.DB;
using charity_website_backend.Entities;
using System.Diagnostics;

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
        public  IResult<ListVM<ProjectListDTO>> GetApprovedProjects(string search,string ngoName, int skip, int take)
        {
            List<ProjectListDTO> datalist = new List<ProjectListDTO>();
            var projects = _context.Projects.Where(x => x.Status == ProjectStatus.Approved && x.Title.ToLower().Contains(search)).ToList();
            var filteredProjects = projects.Where(x => _context.NGOs.Find(x.NGO_Id)?.Name.ToLower().Contains(ngoName) ?? false).ToList();
            if (filteredProjects.Any())
            {
                var data = (from c in filteredProjects.Skip(skip).Take(take)
                            select new ProjectListDTO()
                            {
                                Id = c.Id,
                                ImagePath = c.Image_Path,
                                Title = c.Title,
                                AmountRaised = c.Amount_Raised,
                                TargetAmount = c.Target_Amount
                            }).AsQueryable();
                datalist.AddRange(data);
            }
            return new IResult<ListVM<ProjectListDTO>>()
            {
                Data = new ListVM<ProjectListDTO>(){
                    list = datalist,
                    count = filteredProjects.Count
                },
                Status = status.Success
            };
        }
        public IResult<ListVM<PendingProjectListDTO>> GetPendingProjects(string search,string ngoName, int skip, int take)
        {
            List<PendingProjectListDTO> datalist = new List<PendingProjectListDTO>();
            var projects = _context.Projects.Where(x => x.Status == ProjectStatus.Pending && x.Title.ToLower().Contains(search)).ToList();
            var filteredProjects = projects.Where(x => _context.NGOs.Find(x.NGO_Id)?.Name.ToLower().Contains(ngoName)??false).ToList();
            if (filteredProjects.Any())
            {
                var data = (from c in filteredProjects.Skip(skip).Take(take)
                            select new PendingProjectListDTO()
                            {
                                Id = c.Id,
                                ImagePath = c.Image_Path??"",
                                Title = c.Title,
                                NGOName = _context.NGOs.Find(c.NGO_Id)?.Name??"",
                                TargetAmount = c.Target_Amount
                            }).AsQueryable();
                datalist.AddRange(data);
            }
            
            return new IResult<ListVM<PendingProjectListDTO>>()
            {
                Data = new ListVM<PendingProjectListDTO>()
                {
                    list=datalist,
                    count = filteredProjects.Count
                },
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
                if(project.Amount_Raised >= project.Target_Amount)
                {
                    project.Status = ProjectStatus.Completed;
                    _context.SaveChanges();
                }
                donor.Balance -= model.Amount;
                _context.SaveChanges();
                var donation = new EDonation()
                {
                    Donor_Id = donorId,
                    Amount = model.Amount,
                    Date_And_Time = DateTime.Now,
                    Project_Id = project.Id
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
        public IResult<ListVM<ProjectListDTO>> GetProjectsByNGOId(int NGOId,string search,int skip,int take)
        {
            List<ProjectListDTO> datalist = new List<ProjectListDTO>();
            var projects = _context.Projects.Where(x => x.NGO_Id == NGOId && x.Title.ToLower().Contains(search)).ToList();
            var data = (from c in projects.Skip(skip).Take(take)
                        select new ProjectListDTO()
                        {
                            Id = c.Id,
                            ImagePath = c.Image_Path,
                            Title = c.Title,
                            AmountRaised = c.Amount_Raised,
                            TargetAmount = c.Target_Amount
                        }).AsQueryable();
            datalist.AddRange(data);
            return new IResult<ListVM<ProjectListDTO>>()
            {
                Data = new ListVM<ProjectListDTO>()
                {
                    list = datalist,
                    count = projects.Count
                },
                Status = status.Success
            };
        }

        public IResult<ListVM<DonationHistoryVM>> GetDonationHistory(string projectName, string ngoName, string donorName, int skip, int take)
        {
            List<DonationHistoryVM> list = new List<DonationHistoryVM>();
            var projects = _context.Projects.ToList();
            var ngos = _context.NGOs.ToList();
            var donors = _context.Donors.ToList();
            var donations = _context.Donations.ToList();
            var dataList = (from dns in donations
                         join p in projects on dns.Project_Id equals p.Id into dnsp
                         from dp in dnsp
                         join n in ngos on dp.NGO_Id equals n.Id into dpn
                         from dpns in dpn
                         join d in donors on dns.Donor_Id equals d.Id
                         select new DonationHistoryVM()
                         {
                             Id = dns.Id,
                             Amount = dns.Amount,
                             DateTime = dns.Date_And_Time,
                             DonorId = d.Id,
                             DonorUsername = d.Username,
                             NGOId = dpns.Id,
                             NGOUsername = dpns.Username,
                             ProjectId = dp.Id,
                             ProjectName = dp.Title,
                             ProjectImg = dp.Image_Path
                         }).ToList();
            var datas = dataList.Where(x => x.NGOUsername.ToLower().Contains(ngoName) && x.ProjectName.ToLower().Contains(projectName) && x.DonorUsername.ToLower().Contains(donorName)).OrderByDescending(x=>x.DateTime).ToList();
            list.AddRange(datas.Skip(skip).Take(take));
            ListVM<DonationHistoryVM> dataModel = new ListVM<DonationHistoryVM>()
            {
                list = list,
                count = datas.Count
            };
            return new IResult<ListVM<DonationHistoryVM>>()
            {
                Data = dataModel,
                Status = status.Success,
                Message = "Donation history retrieved successfully"
            };

        }
    }
}
