using charity_website_backend.Entities;
using charity_website_backend.Modules.LoginSignup.Services;

namespace charity_website_backend.Modules.NGO.Services
{
    public interface INGOService
    {
        public IResult<IQueryable<SignUpRequestsDTO>> GetSignUpRequests();
        public IResult<bool> SendVerificationLink(int NGOId);
        public IResult<bool> VerifyNGO(int NGOId);


    }
    public class SignUpRequestsDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
