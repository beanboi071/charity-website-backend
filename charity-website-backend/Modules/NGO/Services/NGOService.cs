using charity_website_backend.Common.Model;
using charity_website_backend.Common.Services;
using charity_website_backend.DB;
using charity_website_backend.Entities;
using charity_website_backend.Modules.Donor.Services;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;

namespace charity_website_backend.Modules.NGO.Services
{
    public class NGOService:INGOService
    {
        private readonly IConfiguration _config;
        
        private readonly CharityDbContext _context;
        public NGOService(CharityDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public IResult<IQueryable<SignUpRequestsDTO>> GetSignUpRequests()
        {
            var data =  (from c in _context.NGOs.Where(x=>x.IsEmailVerified == false)
                         select new SignUpRequestsDTO()
                         {
                             Id = c.Id,
                             Username = c.Username,
                             Name = c.Name,
                             Email = c.Email,
                             Website = c.Website_Link
                             
                         }).AsQueryable();
            return new IResult<IQueryable<SignUpRequestsDTO>>()
            {
                Data = data,
                Status = status.Success
            };
        }

        public IResult<bool> SendVerificationLink(int NGOId)
        {
            var ngo = _context.NGOs.Find(NGOId);
            string htmlTemplate = File.ReadAllText("Common/Template/NGOVerificationTemplate.html");

            string body = string.Format(htmlTemplate, _config["BaseUrl"] + "/VerifyNGO/"+NGOId);
            IEmailService mailService = new EmailService(_config);
            var mailDto = new EmailSendDTO()
            {
                To = ngo.Email,
                Subject = "Signup Verification",
                Body = body
            };
            mailService.SendEmail(mailDto);
            return new IResult<bool>()
            {
                Data = true,
                Status = status.Success,
                Message = "Verification link sent successfully."
            };
        }
        public IResult<bool> VerifyNGO(int NGOId)
        {
            var ngo = _context.NGOs.Find(NGOId);
            ngo.IsEmailVerified = true;
            _context.SaveChanges();
            return new IResult<bool>()
            {
                Data = true,
                Status = status.Success,
                Message = "Verification successful."
            };
        }
        public IResult<NGOProfileVM> GetUserProfile(int userId)
        {
            var ngo = _context.NGOs.Find(userId);
            var data = new NGOProfileVM()
            {
                Id = ngo.Id,
                Balance = ngo.Balance,
                Email = ngo.Email,
                Image_Path = ngo.Image_Path ?? "Default\\OrgImg.jpg",
                Name = ngo.Name ?? "",
                Username = ngo.Username,
                Website_Link = ngo.Website_Link
            };
            return new IResult<NGOProfileVM>()
            {
                Data = data,
                Status = status.Success
            };
        }

        public IResult<int> UpdateUserProfile(NGOProfileVM model, int userId)
        {

            var ngo = _context.NGOs.Find(userId);
            var existingNGOWithUname = _context.NGOs.FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower());
            if (existingNGOWithUname != null && existingNGOWithUname.Id != ngo.Id)
            {
                return new IResult<int>()
                {
                    Status = status.Failure,
                    Message = "Username already in use"
                };
            }
            var existingNGOWithEmail = _context.NGOs.FirstOrDefault(x => x.Email == model.Email);
            if (existingNGOWithEmail != null && existingNGOWithEmail.Id != ngo.Id)
            {
                return new IResult<int>()
                {
                    Status = status.Failure,
                    Message = "This email is already bound to an account"
                };
            }
            if (model.Image_Path != null)
            {
                if (model.Image_Path == "")
                {
                    ngo.Image_Path.DeleteFile();
                    ngo.Image_Path = null;
                }
                if (model.Image_Path.StartsWith("data:image/"))
                {
                    if (!ngo.Image_Path.IsNullOrEmpty())
                    {
                        ngo.Image_Path.DeleteFile();
                    }
                    var fileName = model.Image_Path.ToFileUrl("NGO", Guid.NewGuid().ToString());
                    ngo.Image_Path = fileName;

                }
            }
           
            ngo.Username = model.Username;
            ngo.Name = model.Name;
            ngo.Email = model.Email;
            _context.NGOs.Update(ngo);
            _context.SaveChanges();
            return new IResult<int>()
            {
                Data = ngo.Id,
                Status = status.Success,
                Message = "User profile updated successfully"
            };
        }
        public IResult<NGOListVM> GetNGODetail(int ngoId)
        {
            var ngo = _context.NGOs.Find(ngoId);
            if(ngo!= null)
            {
                return new IResult<NGOListVM>()
                {
                    Data = new NGOListVM()
                    {
                        Id = ngo.Id,
                        Email = ngo.Email,
                        Image_Path = ngo.Image_Path ?? "Default\\OrgImg.jpg",
                        Name = ngo.Name,
                        Username = ngo.Username,
                        Website_Link = ngo.Website_Link
                    },
                    Message = "NGO detail retrieved successfully",
                    Status = status.Success
                };
            }
            return new IResult<NGOListVM>()
            {
                Message = "NGO not found.",
                Status = status.Failure
            };
        }

        public IResult<ListVM<NGOListVM>> List(string search, int skip, int take)
        {
            var list = new List<NGOListVM>();
            var ngos = _context.NGOs.Where(x=>x.Name.ToLower().Contains(search) || x.Username.ToLower().Contains(search)).ToList();
            var datas = (from c in ngos.Skip(skip).Take(take)
                         select new NGOListVM()
                         {
                             Id = c.Id,
                             Email = c.Email,
                             Image_Path = c.Image_Path ?? "Default\\OrgImg.jpg",
                             Name = c.Name,
                             Username = c.Username,
                             Website_Link = c.Website_Link
                         }).ToList();
            list.AddRange(datas);
            var result = new ListVM<NGOListVM>()
            {
                list = list,
                count = ngos.Count
            };
            return new IResult<ListVM<NGOListVM>>()
            {
                Data = result,
                Message = "NGO list retrieved successfully",
                Status = status.Success
            };
        }
    }
}

