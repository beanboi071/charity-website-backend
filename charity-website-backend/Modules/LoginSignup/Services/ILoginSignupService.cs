using charity_website_backend.Entities;

namespace charity_website_backend.Modules.LoginSignup.Services
{
    public interface ILoginSignupService
    {
        IResult<bool> SignUpDonor(SignUpDonorDTO SignUpData);
        IResult<bool> GetOTP(ResetPasswordDTO ResetPasswordData);
        IResult<bool> ChangePassword(ChangePasswordDTO ChangePasswordData, int UserId, int UserType);
        IResult<bool> SignUpNGO(SignUpNGODTO SignUpData);
        IResult<string> Login(LoginDTO LoginData);
        IResult<string> AdminLogin(AdminLoginDTO LoginData);
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
    public class ChangePasswordDTO
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public int UserType { get; set; }
    }
}
