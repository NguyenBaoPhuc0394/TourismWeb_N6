using TourismWeb.DTOs.Image;

namespace TourismWeb.Services.Interfaces
{
    public interface IImageService
    {
        Task<ImageDTO> CreateImage(ImageDTO image);
        Task DeleteImageById(string id);
        Task<IEnumerable<ImageDTO>> GetAllImages();
        Task<ImageDTO> GetImageById(string id);
        Task<IEnumerable<ImageTourDTO>> GetImagesByTourId(string tourId);
        Task<ImageDTO> UpdateImageById(string id, ImageDTO updatedImage);
    }
}
