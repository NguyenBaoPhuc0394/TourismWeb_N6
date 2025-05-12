using TourismWeb.DTOs.Image;

namespace TourismWeb.Services.Interfaces
{
    public interface IImageService
    {
        Task<ImageDTO> CreateImage(string tourId, IFormFile file);
        Task<bool> DeleteImageById(string id);
        Task<IEnumerable<ImageDTO>> GetAllImages();
        Task<ImageDTO> GetImageById(string id);
        Task<IEnumerable<ImageTourDTO>> GetImagesByTourId(string tourId);
        Task<ImageDTO> UpdateImageById(string id, ImageDTO updatedImage);
        Task<IEnumerable<ImageDTO>> GetImagesDataByTourId(string tourId);
    }
}
