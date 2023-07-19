using charity_website_backend.Entities;
using charity_website_backend.Modules.LoginSignup.Services;

namespace charity_website_backend.Modules.LoginSignup.Api
{
    public static class LoginSignupApi
    {
        public static void RegisterLoginSignupApi(this WebApplication app)
        {
            var root = "Api/";
            app.MapPost(root + "SiginUp", SignUp);
            app.MapPost(root + "Login", Login);
        }
        private static IResult<bool> SignUp(SignUpDTO SignupData,ILoginSignupService service)
        {
            return service.SignUp(SignupData);
        }
        private static IResult<string> Login(LoginDTO LoginData, ILoginSignupService service)
        {
            return service.Login(LoginData);
        }
    }
}
