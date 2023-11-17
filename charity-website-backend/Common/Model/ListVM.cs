namespace charity_website_backend.Common.Model
{
    public class ListVM<t>
    {
        public ICollection<t> list { get; set; }
        public int count { get; set; }
    }
}