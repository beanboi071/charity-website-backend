namespace charity_website_backend.Common.Services
{
    public class CommonService:ICommonService
    {
        public async Task GetImageFile(string fileName, HttpContext context)
        {
            // Define a custom directory name to store images
            string customDirectoryName = "uploads";
            string imageDirectory = Path.Combine(AppContext.BaseDirectory, customDirectoryName);


            // Create the custom directory if it does not exist
            Directory.CreateDirectory(imageDirectory);

            string filePath = Path.Combine(imageDirectory, fileName);

            if (File.Exists(filePath))
            {
                await context.Response.SendFileAsync(filePath);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
