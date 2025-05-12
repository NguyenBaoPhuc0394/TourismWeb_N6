using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class CloudinaryService: ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new CloudinaryDotNet.Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public string ToUrlImage(string publicId)
        {

            string format = "jpg";
            var url = _cloudinary.Api.UrlImgUp.BuildUrl($"{publicId}.{format}");
            return url;
        }

        public async Task<bool> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok" || result.Result == "deleted";
        }

        public async Task<string?> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "upload-tour",
                UseFilename = false,
                UniqueFilename = false,
                Overwrite = false,
                Format = "jpg"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.PublicId;
            }
            return null;
        }

    }
}
