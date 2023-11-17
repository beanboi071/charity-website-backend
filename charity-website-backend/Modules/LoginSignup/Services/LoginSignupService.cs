using charity_website_backend.Common.Services;
using charity_website_backend.DB;
using charity_website_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

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
        public IResult<string> AdminLogin(AdminLoginDTO LoginData)
        {
            var admin = _context.Admin.FirstOrDefault(x => x.Username == LoginData.Username);
            if (admin == null || (!BCrypt.Net.BCrypt.Verify(LoginData.Password, admin.Password_Hash)))
            {
                return new IResult<string>()
                {
                    Status = status.Failure,
                    Message = "Invalid login credential."
                };
            }
            string adminToken = CreateToken(admin.Id, 3);
            return new IResult<string>()
            {
                Data = adminToken,
                Status = status.Success,
                Message = "Login Successful"
            };
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
                    if (!NGO.IsEmailVerified)
                    {
                        return new IResult<string>()
                        {
                            Status = status.Failure,
                            Message = "Please verify your email."
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

                Expires = DateTime.Now.AddHours(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public IResult<bool> SignUpDonor(SignUpDonorDTO SignUpData)
        {
            var existingDonor = _context.Donors.FirstOrDefault(x => x.Username == SignUpData.Username);
            if (existingDonor != null)
            {
                return new IResult<bool>()
                {
                    Status = status.Failure,
                    Message = "Username is already taken."
                };
            }
            var existingEmail = _context.Donors.FirstOrDefault(x => x.Email == SignUpData.Email);
            if (existingEmail != null)
            {
                return new IResult<bool>()
                {
                    Status = status.Failure,
                    Message = "Email already in use."
                };
            }
            var newDonor = new EDonor()
            {
                Name = SignUpData.FullName,
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
        }
        public IResult<bool> ChangePassword(ChangePasswordDTO ChangePasswordData,int UserId,int UserType)
        {
            switch (UserType)
            {
                case 0:
                    var donor = _context.Donors.Find(UserId);
                    if (donor == null || (!BCrypt.Net.BCrypt.Verify(ChangePasswordData.OldPassword, donor.Password_Hash)))
                    {
                        return new IResult<bool>()
                        {
                            Status = status.Failure,
                            Message = "Incorrect password."
                        };
                    }
                    donor.Password_Hash = BCrypt.Net.BCrypt.HashPassword(ChangePasswordData.OldPassword);
                    _context.SaveChanges();
                    return new IResult<bool>()
                    {
                        Status= status.Success,
                        Message = "Password changed successfully"
                    };
                case 1:
                    var ngo = _context.NGOs.Find(UserId);
                    if (ngo == null || (!BCrypt.Net.BCrypt.Verify(ChangePasswordData.OldPassword, ngo.Password_Hash)))
                    {
                        return new IResult<bool>()
                        {
                            Status = status.Failure,
                            Message = "Incorrect password."
                        };
                    }
                    ngo.Password_Hash = BCrypt.Net.BCrypt.HashPassword(ChangePasswordData.NewPassword);
                    _context.SaveChanges();
                    return new IResult<bool>()
                    {
                        Status = status.Success,
                        Message = "Password changed successfully"
                    };
                default:
                    return new IResult<bool>()
                    {
                        Status = status.Failure,
                        Message = "Something went wrong"
                    };
            }
        }
        public IResult<bool> SignUpNGO(SignUpNGODTO SignUpData)
        {
            //IEmailService mailService = new EmailService(_config);
            //var mailDto = new EmailSendDTO()
            //{
            //    To = "rojanshakya071@gmail.com",
            //    Subject = "OTP for Email Verification",
            //    Body = "hi"
            //};
            
            
                    var existingNGO = _context.NGOs.FirstOrDefault(x => x.Username == SignUpData.Username);
                    if (existingNGO != null)
                    {
                        return new IResult<bool>()
                        {
                            Data = true,
                            Status = status.Failure,
                            Message = "Username is already taken."
                        };
                    }
                    var existingEmail = _context.NGOs.FirstOrDefault(x => x.Email == SignUpData.Email);
            if (existingEmail != null)
            {
                if (existingEmail.IsEmailVerified)
                {
                    return new IResult<bool>()
                    {
                        Data = true,
                        Status = status.Failure,
                        Message = "Email is already in use."
                    };
                }
                else
                {
                    _context.NGOs.Remove(existingEmail);
                    _context.SaveChanges();
                }
            }
            var newNGO = new ENGO()
                    {
                Name = SignUpData.NGOName,
                        Username = SignUpData.Username,
                        Email = SignUpData.Email,
                        Balance = 0,
                        Password_Hash = BCrypt.Net.BCrypt.HashPassword(SignUpData.Password),
                        Website_Link = SignUpData.Website,
                        IsEmailVerified = false,
            };
                    var registerNGO = _context.NGOs.Add(newNGO);
                    _context.SaveChanges();
                    return new IResult<bool>()
                    {
                        Status = status.Success,
                        Message = "Signup successful"
                    };
            
        }
        public string GenerateOtpCode()
        {
            // Define the length of the OTP code
            int otpLength = 6;

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[otpLength];
                rng.GetBytes(bytes);

                // Convert the random bytes to a numeric OTP code
                int number = BitConverter.ToInt32(bytes, 0) & int.MaxValue;

                // Scale the number to the desired length
                int maxDigits = (int)Math.Pow(10, otpLength);
                int otpCode = number % maxDigits;

                // Convert the OTP code to a string and pad it with leading zeros if necessary
                return otpCode.ToString().PadLeft(otpLength, '0');
            }
        }
        public IResult<bool> VerifyOTP(VerifyOtpDTO model)
        {
            int? UserId = null;
            switch (model.UserType)
            {
                case 0:
                    UserId = _context.Donors.FirstOrDefault(x => x.Email == model.Email)?.Id;
                    break;
                case 1:
                    UserId = _context.NGOs.FirstOrDefault(x => x.Email == model.Email)?.Id;
                    break;
                default:
                    break;
            }
            if (UserId == null)
            {
                return new IResult<bool>
                {
                    Data = false,
                    Status = status.Failure,
                    Message = "User not found."
                };
            }
            var otp = _context.OTPs.FirstOrDefault(x => x.UserId == UserId && x.UserType == model.UserType && x.Otp == model.OTP);
            if(otp != null)
            {
                if (DateTime.Now > otp.ExpiryTime)
                {
                    string query = "DELETE FROM OTPs WHERE UserId = @p0 AND UserType = @p1;";
                    _context.Database.ExecuteSqlRaw(query, otp.UserId,otp.UserType );
                    return new IResult<bool>()
                    {
                        Data = false,
                        Message = "OTP Expired",
                        Status = status.Failure
                    };
                }
                string DeleteQuery = "DELETE FROM OTPs WHERE UserId = @p0 AND UserType = @p1;";
                _context.Database.ExecuteSqlRaw(DeleteQuery, otp.UserId, otp.UserType);
                return new IResult<bool>
                {
                    Data = true,
                    Message = "OTP verified",
                    Status = status.Success
                };
            }
            
            return new IResult<bool>
            {
                Data = false,
                Message = "Failed to verify OTP",
                Status = status.Failure
            };
        }
            public IResult<bool> GetOTP(ResetPasswordDTO ResetPasswordData)
        {
            int? UserId = null;
            switch (ResetPasswordData.UserType)
            {
                case 0:
                    UserId = _context.Donors.FirstOrDefault(x => x.Email == ResetPasswordData.Email)?.Id;
                    break;
                case 1:
                    UserId = _context.NGOs.FirstOrDefault(x => x.Email == ResetPasswordData.Email)?.Id;
                    break;
                default:
                    break;
            }
            if(UserId == null)
            {
                return new IResult<bool> 
                {
                    Data = false,
                    Status = status.Failure,
                    Message = "User not found."
                };
            }
            var otp = GenerateOtpCode();
            string htmlTemplate = System.IO.File.ReadAllText("Common/Template/ResetPasswordTemplate.html");

            string body = string.Format(htmlTemplate, otp);
            IEmailService mailService = new EmailService(_config);
            var mailDto = new EmailSendDTO()
            {
                To = ResetPasswordData.Email,
                Subject = "OTP to reset password",
                Body = body
            };

            mailService.SendEmail(mailDto);
            string DeleteQuery = "DELETE FROM OTPs WHERE UserId = @p0 AND UserType = @p1;";
            _context.Database.ExecuteSqlRaw(DeleteQuery, UserId ?? 0, ResetPasswordData.UserType);
            string query = "INSERT INTO OTPs (UserId,UserType,Otp,ExpiryTime) VALUES (@p0, @p1, @p2,@p3);";
            _context.Database.ExecuteSqlRaw(query, UserId ?? 0, ResetPasswordData.UserType,otp,DateTime.Now.AddMinutes(1));
            return new IResult<bool>()
            {
                Data = true,
                Status = status.Success,
                Message = "OTP sent successfully"
            };
        }

        public IResult<bool> DeleteOTP(int UserType, string Email)
        {
            int? UserId = null;
            switch (UserType)
            {
                case 0:
                    UserId = _context.Donors.FirstOrDefault(x => x.Email == Email)?.Id;
                    break;
                case 1:
                    UserId = _context.NGOs.FirstOrDefault(x => x.Email == Email)?.Id;
                    break;
                default:
                    break;
            }
            string query = "DELETE FROM OTPs WHERE UserId = @p0 AND UserType = @p1;";
            _context.Database.ExecuteSqlRaw(query, UserId, UserType);
            return new IResult<bool>
            {
                Data = true,
                Status = status.Success
            };
        }
    }
}
