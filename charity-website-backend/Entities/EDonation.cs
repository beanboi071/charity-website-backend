namespace charity_website_backend.Entities
{
    public class EDonation
    {
        public int Id { get; set; }
        public int Donor_Id { get; set; }
        public int Project_Id { get; set; }
        public DateTime Date_And_Time{ get; set; }
        public decimal Amount { get; set; }
    }
}
