using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
namespace charity_website_backend.Common.Services
{
    public static class FileExtension
    {
        public static string ToFileUrl(this string base64String, string folder, string filename)
        {
            // Determine the file extension based on the image format
            // Determine the file extension based on the image format
            try
            {
                if (string.IsNullOrEmpty(base64String))
                {
                    return string.Empty;
                }
                if (base64String.Contains(string.Format("{0}/{1}", folder, filename)))
                {
                    return base64String;
                }
                string[] base64strs = base64String.Split(',');
                if (base64strs.Length > 1)
                {
                    base64String = base64String.Split(',')[1];
                }
                string extension = GetFileExtension(base64String);

                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Get the desired filename from user input or some other source
                string rootDirectory = "C:\\Users\\Riddhasoft-002\\Documents\\Rojan ko project\\charity-website-backend\\charity-website-backend\\wwwroot\\images";
                filename += extension;
                string fullPath = Path.Combine(rootDirectory, folder, filename);
                if (!Directory.Exists(Path.Combine(rootDirectory, folder)))
                {
                    Directory.CreateDirectory(Path.Combine(rootDirectory, folder));
                }
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                using (var ms = new MemoryStream(imageBytes))
                {
                    using (var image = Image.Load<Rgba32>(ms))
                    {
                        // Save the image to a file with the appropriate extension
                        image.Save(fullPath, GetEncoder(extension));
                    }
                }
                return Path.Combine(folder, filename);
            }
            catch (Exception ex)
            {
                //log exception here
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(this string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                //logger here for exception stack trace
                throw new Exception(ex.Message);
            }

        }
        static string GetFileExtension(string base64String)
        {
            string[] formats = { "jpeg", "png", "gif" };
            int headerLength = 20; // adjust based on the expected header length
            byte[] imageBytes = Convert.FromBase64String(base64String);

            // Examine the first few bytes of the Base64 string to determine the file format
            var header = BitConverter.ToString(imageBytes.Take(headerLength).ToArray());
            string format = formats.FirstOrDefault(f => header.StartsWith(GetHeader(f)));

            if (format == null)
            {
                throw new NotSupportedException("Unsupported image format.");
            }

            // Map the file format to the corresponding file extension
            switch (format)
            {
                case "jpeg":
                    return ".jpg";
                case "png":
                    return ".png";
                case "gif":
                    return ".gif";
                default:
                    throw new NotSupportedException("Unsupported image format.");
            }

        }

        static string GetHeader(string format)
        {
            // Return the expected header for the specified image format
            switch (format)
            {
                case "jpeg":
                    return "FF-D8-FF";
                case "png":
                    return "89-50-4E";
                case "gif":
                    return "47-49-46";
                default:
                    throw new NotSupportedException("Unsupported image format.");
            }
        }

        static IImageEncoder GetEncoder(string extension)
        {
            // Return the appropriate image encoder based on the file extension
            switch (extension)
            {
                case ".jpg":
                    return new JpegEncoder();
                case ".png":
                    return new PngEncoder();
                case ".gif":
                    return new GifEncoder();
                default:
                    throw new NotSupportedException("Unsupported file extension.");
            }
        }
    }
}
