﻿using charity_website_backend.Common.Model;
using charity_website_backend.Entities;

namespace charity_website_backend.Modules.Project.Services
{
    public interface IProjectService
    {
        IResult<EProject> Add(ProjectCreateDTO model,int NGOId);
        IResult<bool> DonateToProject(DonationDTO model, int donorId);
        IResult<ListVM<ProjectListDTO>> GetProjectsByNGOId(int NGOId,string search,int skip,int take);
        IResult<ListVM<ProjectListDTO>> GetApprovedProjects(string search,string ngoName, int skip, int take);
        IResult<bool> ApproveProject(int ProjectId);
        IResult<bool> RejectProject(int ProjectId);
        IResult<ListVM<ProjectListDTO>> GetPendingProjects(string search,string ngoName, int skip, int take);
        
        IResult<ProjectDetailDTO> GetProjectDetails(int projectid);
        IResult<ListVM<DonationHistoryVM>> GetDonationHistory(string projectName, string ngoName, string donorName, int skip, int take);
    }
    public class DonationDTO
    {
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
    
    }
    public class DonationHistoryVM
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public string DonorUsername { get; set; }
        public int NGOId { get; set; }
        public string NGOUsername { get; set; }
        public decimal Amount { get; set; }
        public string DateTime { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectImg { get; set; }
        
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
        public string? NGOName { get; set; }

    }
    public class ProjectDetailDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImagePath{ get; set; }
        public decimal AmountRaised { get; set; }
        public decimal TargetAmount { get; set; }
        public string CreatedDateTime { get; set; }
        public int NGOId { get; set; }
        public string NGOName { get; set; }
        public string NGOUsername { get; set; }
        public string Website_Link { get; set; }

        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
