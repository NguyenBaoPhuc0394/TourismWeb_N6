using CloudinaryDotNet;
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
    }
}
