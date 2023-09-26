using charity_website_backend.Entities;

namespace charity_website_backend.Modules.LoginSignup.Services
{
    public interface ILoginSignupService
    {
        public IResult<bool> SignUpDonor(SignUpDonorDTO SignUpData);
        public IResult<bool> SignUpNGO(SignUpNGODTO SignUpData);
        public IResult<string> Login(LoginDTO LoginData);
        public IResult<string> AdminLogin(AdminLoginDTO LoginData);
    }
   public class SignUpDonorDTO
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPwd { get; set; }
    }
    public class SignUpNGODTO
    {
        public string NGOName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPwd { get; set; }
        public string Website { get; set; }
    }
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
    }
    public class AdminLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


}
