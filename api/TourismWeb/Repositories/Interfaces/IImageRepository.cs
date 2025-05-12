using TourismWeb.DTOs.Image;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAllImages();
        Task<Image> GetImageById(string id);
        Task<IEnumerable<Image>> GetImagesByTourId(string tourId);
        Task<Image> CreateImage(string tourId, string publicKey);
        Task<Image> UpdateImageById(string id, Image imageDto);
        Task<bool> DeleteImageById(string id);
        Task<string?> GetImageUrl(string id);
    }
}
