using TourismWeb.DTOs.Image;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAllImages();
        Task<Image> GetImageById(string id);
        Task<IEnumerable<Image>> GetImagesByTourId(string tourId);
        Task<Image> CreateImage(Image imageDto);
        Task<Image> UpdateImageById(string id, Image imageDto);
        Task DeleteImageById(string id);
    }
}
