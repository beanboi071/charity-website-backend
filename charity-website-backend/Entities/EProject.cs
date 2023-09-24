namespace charity_website_backend.Entities
{
    public class EProject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int NGO_Id { get; set; }
        public string? Description { get; set; }
        public string? Image_Path { get; set; }
        public DateTime Created_Date_And_Time { get; set; }
        public decimal Target_Amount { get; set; }
        public decimal Amount_Raised { get; set; }
        public ProjectStatus Status { get; set; }
    }
    public enum ProjectStatus
    {
        Pending,
        Approved,
        Completed,
        Rejected
    }
}
