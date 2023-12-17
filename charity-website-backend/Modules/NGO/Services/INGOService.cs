using charity_website_backend.Common.Model;
using charity_website_backend.Entities;
using charity_website_backend.Modules.Donor.Services;
using charity_website_backend.Modules.LoginSignup.Services;

namespace charity_website_backend.Modules.NGO.Services
{
    public interface INGOService
    {
        IResult<IQueryable<SignUpRequestsDTO>> GetSignUpRequests();
        IResult<bool> SendVerificationLink(int NGOId);
        IResult<bool> VerifyNGO(int NGOId);
        IResult<NGOProfileVM> GetUserProfile(int userId);
        IResult<int> UpdateUserProfile(NGOProfileVM model, int userId);
        IResult<ListVM<NGOListVM>> List(string search, int skip, int take);
        IResult<NGOListVM> GetNGODetail(int ngoId);
    }
    public class SignUpRequestsDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
    public class NGOProfileVM
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Image_Path { get; set; }
        public decimal Balance { get; set; }
        public string Website_Link { get; set; }
    }
    public class NGOListVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Image_Path { get; set; }
        public string Website_Link { get; set; }
    }
}
