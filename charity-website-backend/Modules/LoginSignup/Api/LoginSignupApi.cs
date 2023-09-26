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
        }
        private static IResult<bool> SignUpDonor(SignUpDonorDTO SignupData,ILoginSignupService service)
        {
            return service.SignUpDonor(SignupData);
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
