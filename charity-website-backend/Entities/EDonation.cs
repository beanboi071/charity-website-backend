namespace charity_website_backend.Entities
{
    public class EDonation
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateAndTime{ get; set; }
        public decimal Amount { get; set; }
    }
}
