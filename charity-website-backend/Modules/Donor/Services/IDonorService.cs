using charity_website_backend.Common.Services;
using charity_website_backend.Entities;

namespace charity_website_backend.Modules.Donor.Services
{
    public interface IDonorService
    {
        IResult<DonorProfileVM> GetUserProfile(int userId);
        IResult<int> UpdateUserProfile(DonorProfileVM model, int userId);
    }
    public class DonorProfileVM
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Image_Path { get; set; }
        public decimal? Balance { get; set; }
    }
}
