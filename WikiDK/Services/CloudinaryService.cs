using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace WikiDK.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(config["CLOUDINARY_CLOUD_NAME"], config["CLOUDINARY_API_KEY"], config["CLOUDINARY_API_SECRET"]);

            this.cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImage(IFormFile imgFile)
        {
            if (imgFile.Length <= 0)
                return null;

            await using var stream = imgFile.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imgFile.FileName, stream)
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }
    }
}
