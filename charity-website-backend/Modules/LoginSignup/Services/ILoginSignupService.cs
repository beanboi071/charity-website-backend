using charity_website_backend.Entities;

namespace charity_website_backend.Modules.LoginSignup.Services
{
    public interface ILoginSignupService
    {
        public IResult<bool> SignUp(SignUpDTO SignUpData);
        public IResult<string> Login(LoginDTO LoginData);
    }
   public class SignUpDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPwd { get; set; }
        public char UserType { get; set; }
    }
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public char UserType { get; set; }
    } 
}
