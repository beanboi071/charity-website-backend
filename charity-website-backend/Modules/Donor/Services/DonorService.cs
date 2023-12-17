using charity_website_backend.Common.Services;
using charity_website_backend.DB;
using charity_website_backend.Entities;
using Microsoft.IdentityModel.Tokens;

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

        public IResult<int> UpdateUserProfile(DonorProfileVM model, int userId)
        {
            
            var donor = _context.Donors.Find(userId);
            var existingDonorWithUname = _context.Donors.FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower());
            if (existingDonorWithUname != null && existingDonorWithUname.Id!= donor.Id)
            {
                return new IResult<int>()
                {
                    Status = status.Failure,
                    Message = "Username already in use"
                };
            }
            var existingDonorWithEmail= _context.Donors.FirstOrDefault(x => x.Email == model.Email);
            if (existingDonorWithEmail != null && existingDonorWithEmail.Id != donor.Id)
            {
                return new IResult<int>()
                {
                    Status = status.Failure,
                    Message = "This email is already bound to an account"
                };
            }
            if(model.Image_Path != null)
            {
                if(model.Image_Path == "")
                {
                    donor.Image_Path.DeleteFile();
                    donor.Image_Path = null;        
                }
                if (model.Image_Path.StartsWith("data:image/"))
                {
                    if (!donor.Image_Path.IsNullOrEmpty())
                    {
                        donor.Image_Path.DeleteFile();
                    }
                    var fileName = model.Image_Path.ToFileUrl("Donor", Guid.NewGuid().ToString());
                    donor.Image_Path = fileName;

                }
            }
            
            donor.Username = model.Username;
            donor.Name = model.Name;
            donor.Email = model.Email;
            _context.Donors.Update(donor);
            _context.SaveChanges();
            return new IResult<int>()
            {
                Data = donor.Id,
                Status = status.Success,
                Message = "User profile updated successfully"
            };
        }
    }
}
