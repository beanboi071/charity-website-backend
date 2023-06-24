using charity_website_backend.DB;
using charity_website_backend.Entities;

namespace charity_website_backend.Modules.LoginSignup.Services
{
    public class LoginSignupService : ILoginSignupService
    {
        private readonly CharityDbContext _context;
        public LoginSignupService(CharityDbContext context)
        {
            _context = context;
        }
        public IResult<bool> SignUp(SignUpDTO SignUpData)
        {
            if (SignUpData == null) 
            {
                return new IResult<bool>
                {
                    Status = status.Failure,
                    Message = "Invalid Signup data"
                };
            }
            switch (SignUpData.UserType)
            {
                case '0':
                    var existingDonor = _context.Donors.FirstOrDefault(x=>x.Username == SignUpData.Username);
                    if (existingDonor != null)
                    {
                        return new IResult<bool>()
                        {
                            Status = status.Failure,
                            Message = "Donor already exists."
                        };
                    }
                    var newDonor = new EDonor()
                    {
                        Username = SignUpData.Username,
                        Email = SignUpData.Email,
                        Balance = 100000,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(SignUpData.Password)
                    };
                    var registerDonor = _context.Donors.Add(newDonor);
                    _context.SaveChanges();
                    return new IResult<bool>()
                    {
                        Data = true,
                        Status = status.Success,
                        Message = "Signup successful"
                    };
                    break;
                case '1':
                    var existingNGO = _context.NGOs.FirstOrDefault(x => x.Username == SignUpData.Username);
                    if (existingNGO != null)
                    {
                        return new IResult<bool>()
                        {
                            Data = true,
                            Status = status.Failure,
                            Message = "Donor already exists."
                        };
                    }
                    var newNGO = new ENGO()
                    {
                        Username = SignUpData.Username,
                        Email = SignUpData.Email,
                        Balance = 0,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(SignUpData.Password)
                    };
                    var registerNGO = _context.NGOs.Add(newNGO);
                    _context.SaveChanges();
                    return new IResult<bool>()
                    {
                        Status = status.Success,
                        Message = "Signup successful"
                    };
                    break;
                default:
                    break;
            }
            return new IResult<bool>()
            {
                Status = status.Failure,
                Message = "Signup unsuccessful"
            };
        }
    }
}
