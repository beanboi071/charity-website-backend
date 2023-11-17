using charity_website_backend.DB;
using charity_website_backend.Entities;

namespace charity_website_backend.Modules.Donor.Services
{
    public class DonorService:IDonorService
    {
        private readonly CharityDbContext _context;
        public DonorService(CharityDbContext context)
        {
            _context = context;
        }

        public IResult<DonorProfileVM> GetUserProfile(int userId)
        {
            var donor = _context.Donors.Find(userId);
            var data = new DonorProfileVM()
            {
                Id = donor.Id,
                Balance = donor.Balance,
                Email = donor.Email,
                Image_Path = donor.Image_Path ?? "Default\\UserProfileImg.jpg",
                Name = donor.Name ?? "",
                Username = donor.Username
            };
            return new IResult<DonorProfileVM>()
            {
                Data = data,
                Status = status.Success
            };
        }
    }
}
