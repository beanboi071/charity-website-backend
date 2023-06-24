namespace charity_website_backend.Entities
{
    public class EProject
    {
        public int Id { get; set; }
        public int NGOId { get; set; }
        public string? Description { get; set; }
        public string? ImageBase64 { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal AmountRaised { get; set; }
        public ProjectStatus Status { get; set; }
    }
    public enum ProjectStatus
    {
        Pending,
        Approved,
        Completed
    }
}
