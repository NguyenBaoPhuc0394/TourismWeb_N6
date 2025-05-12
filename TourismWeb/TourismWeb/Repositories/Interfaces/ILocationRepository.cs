using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location> CreateLocation(Location location);
        Task<IEnumerable<Location>> GetAllLocation();
    }
}
