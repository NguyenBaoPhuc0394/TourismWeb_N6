namespace TourismWeb.Services.Interfaces
{
    public interface ICloudinaryService
    {
        string ToUrlImage(string publicKey);

        Task<bool> DeleteImage(string publicId);

        Task<string?> UploadImage(IFormFile file);
    }
}
