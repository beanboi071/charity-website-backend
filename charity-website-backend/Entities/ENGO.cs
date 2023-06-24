namespace charity_website_backend.Entities
{
    public class ENGO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? ImageBase64 { get; set; }
        public decimal Balance { get; set; }

    }
}
