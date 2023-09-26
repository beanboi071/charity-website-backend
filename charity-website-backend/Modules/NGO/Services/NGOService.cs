using charity_website_backend.Common.Services;
using charity_website_backend.DB;
using charity_website_backend.Entities;

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
    }
}
