namespace charity_website_backend.Common.Services
{
    public interface ICommonService
    {
        Task GetImageFile(string fileName, HttpContext context);
    }
}
