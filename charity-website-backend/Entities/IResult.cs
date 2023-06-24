namespace charity_website_backend.Entities
{
    public class IResult<t>
    {
        public t? Data{ get; set; }
        public status? Status { get; set; }
        public string? Message { get; set; }
    }
    public enum status
    {
        Success,Failure
    }
}
