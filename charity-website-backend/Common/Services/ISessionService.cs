namespace charity_website_backend.Common.Services
{
   
        public interface ISessionService
        {
            ISessionService Session { get; }
            public CharitySession GetSession();
        }

        public class CharitySession
        {
            public int Id { get; set; }
            public int UserType { get; set; }

        }
    
}
