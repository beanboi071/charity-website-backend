namespace charity_website_backend.Common.Services
{
   
        public interface ISessionService
        {
        public HttpContext? HttpContext { get; }
        public IConfiguration Configuration { get; }
        public int Id { get; }
        public int UserType { get; }

    }

    
}
