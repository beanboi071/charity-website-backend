using charity_website_backend.Common.Services;
using charity_website_backend.Entities;
using charity_website_backend.Modules.LoginSignup.Services;

namespace charity_website_backend.Modules.LoginSignup.Api
{
    public static class LoginSignupApi
    {
        public static void RegisterLoginSignupApi(this WebApplication app)
        {
            var root = "Api/";
            app.MapPost(root + "SiginUpDonor", SignUpDonor).AllowAnonymous();
            app.MapPost(root + "SiginUpNGO", SignUpNGO).AllowAnonymous();
            app.MapPost(root + "Login", Login).AllowAnonymous();
            app.MapPost(root + "AdminLogin", AdminLogin).AllowAnonymous();
            app.MapPost(root + "ChangePassword", ChangePassword);
            app.MapPost(root + "GetOTP", GetOTP).AllowAnonymous();
            app.MapPost(root + "VerifyOTP", VerifyOTP).AllowAnonymous();
            app.MapGet(root + "DeleteOTP", DeleteOTP).AllowAnonymous();
            app.MapPost(root + "RecoverPassword", RecoverPassword).AllowAnonymous();
        }
        private static IResult<bool> DeleteOTP(ILoginSignupService service, int UserType, string Email)
        {
            return service.DeleteOTP(UserType, Email);
        }
        private static IResult<bool> SignUpDonor(SignUpDonorDTO SignupData,ILoginSignupService service)
        {
            return service.SignUpDonor(SignupData);
        }
        private static IResult<bool> GetOTP(ResetPasswordDTO ResetPasswordData, ILoginSignupService service)
        {
            return service.GetOTP(ResetPasswordData);
        }
        private static IResult<bool> VerifyOTP(VerifyOtpDTO VerifyOtpData, ILoginSignupService service)
        {
            return service.VerifyOTP(VerifyOtpData);
        }
        private static IResult<bool> ChangePassword(ChangePasswordDTO ChangePasswordData, ILoginSignupService service, ISessionService sessionService)
        {
            int UserId = sessionService.Id;
            int UserType = sessionService.UserType;
            return service.ChangePassword(ChangePasswordData,UserId, UserType);
        }
        private static IResult<bool> RecoverPassword(RecoverPasswordDTO RecoverPasswordData, ILoginSignupService service)
        {
            return service.RecoverPassword(RecoverPasswordData);
        }
        private static IResult<bool> SignUpNGO(SignUpNGODTO SignupData, ILoginSignupService service)
        {
            return service.SignUpNGO(SignupData);
        }
        private static IResult<string> Login(LoginDTO LoginData, ILoginSignupService service)
        {
            return service.Login(LoginData);
        }
        private static IResult<string> AdminLogin(AdminLoginDTO LoginData, ILoginSignupService service)
        {
            return service.AdminLogin(LoginData);
        }
    }
}
