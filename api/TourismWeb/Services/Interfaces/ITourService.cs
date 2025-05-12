using TourismWeb.DTOs.Tour;
using TourismWeb.DTOs.Tours;

namespace TourismWeb.Services.Interfaces
{
    public interface ITourService
    {
        Task<TourDTO> CreateTour(TourCreateDTO tour);
        Task<IEnumerable<TourDTO>> GetAllTours();
        Task<TourDTO> GetTourById(string id);
        Task<TourCreateDTO> UpdateTourById(string id, TourUpdateDTO tour);
        Task DeleteTourById(string id);
        Task<IEnumerable<TourDTO>> GetToursByCategory(string categoryId);

        Task<IEnumerable<TourCardDTO>> GetAllPromotionTours();
        Task<IEnumerable<TourCardDTO>> SearchTours(TourSearchDTO searchData);
        
        Task<IEnumerable<TourTableDTO>> GetAllTourTable();
    }
}
