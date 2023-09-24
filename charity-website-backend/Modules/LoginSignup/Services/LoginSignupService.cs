using charity_website_backend.DB;
using charity_website_backend.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace charity_website_backend.Modules.LoginSignup.Services
{
    public class LoginSignupService : ILoginSignupService
    {
        private readonly IConfiguration _config;
        private readonly CharityDbContext _context;
        public LoginSignupService(CharityDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IResult<string> Login(LoginDTO LoginData)
        {
            switch (LoginData.UserType)
            {
                case 0:
                    var donor = _context.Donors.FirstOrDefault(x => x.Username == LoginData.Username);
                    if(donor == null || (!BCrypt.Net.BCrypt.Verify(LoginData.Password, donor.Password_Hash)))
                    {
                        return new IResult<string>()
                        {
                            Status = status.Failure,
                            Message = "Invalid login credential."
                        };
                    }
                    string donorToken = CreateToken(donor.Id, 0);
                    return new IResult<string>()
                    {
                        Data = donorToken,
                        Status = status.Success,
                        Message = "Login Successful"
                    };
                    break;
                case 1:
                    var NGO = _context.NGOs.FirstOrDefault(x => x.Username == LoginData.Username);
                    if (NGO == null || (!BCrypt.Net.BCrypt.Verify(LoginData.Password, NGO.Password_Hash)))
                    {
                        return new IResult<string>()
                        {
                            Status = status.Failure,
                            Message = "Invalid login credential."
                        };
                    }
                    string ngoToken = CreateToken(NGO.Id, 1);
                    return new IResult<string>()
                    {
                        Data = ngoToken,
                        Status = status.Success,
                        Message = "Login Successful"
                    };
                    break;
                default:
                    break;
            }
            return new IResult<string>()
            {
                Status = status.Failure,
                Message = "Login failed."
            };
        }
        public string CreateToken(int Id, int UserType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_config["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
             {
                  new Claim("Id",Id.ToString(),ClaimValueTypes.Integer),
                  new Claim("UserType",UserType.ToString(),ClaimValueTypes.Integer)
             }),

                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
                case 0:
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
                        Password_Hash = BCrypt.Net.BCrypt.HashPassword(SignUpData.Password)
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
                case 1:
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
                        Password_Hash = BCrypt.Net.BCrypt.HashPassword(SignUpData.Password)
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
