using TourismWeb.DTOs.Tour;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface ITourRepository
    {
        Task<Tour> CreateTour(Tour tour);
        Task<IEnumerable<Tour>> GetAllTours();
        Task<Tour> GetTourById(string id);
        Task<Tour> UpdateTourById(string id, Tour tour);
        Task DeleteTourById(string id);
        Task<IEnumerable<Tour>> GetToursByCategory(string categoryId);
        Task<IEnumerable<TourCardDTO>> GetAllPromotionTours();
        Task<IEnumerable<TourCardDTO>> SearchTours(TourSearchDTO searchData);
    }
}
