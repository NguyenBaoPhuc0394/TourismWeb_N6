using TourismWeb.DTOs.Tour;
using TourismWeb.DTOs.Tours;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface ITourRepository
    {
        Task<Tour> CreateTour(TourCreateDTO tour);
        Task<IEnumerable<Tour>> GetAllTours();
        Task<Tour> GetTourById(string id);
        Task<Tour> UpdateTourById(string id, TourUpdateDTO tour);
        Task DeleteTourById(string id);
        Task<IEnumerable<Tour>> GetToursByCategory(string categoryId);
        Task<IEnumerable<TourCardDTO>> GetAllPromotionTours();
        Task<IEnumerable<TourCardDTO>> SearchTours(TourSearchDTO searchData);
    }
}
