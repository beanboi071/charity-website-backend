using Microsoft.EntityFrameworkCore;

namespace charity_website_backend.Entities
{
    [Keyless]
    public class EOTP
    {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string Otp { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
