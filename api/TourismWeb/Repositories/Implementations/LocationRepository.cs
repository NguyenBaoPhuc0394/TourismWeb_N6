using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.DTOs.Location;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly TourismDbContext _context;

        public LocationRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<Location> CreateLocation(Location location)
        {
            var result = _context.Locations.FromSqlRaw("EXEC CreateLocation " +
                "@Name = {0}", location.Name)
                .AsEnumerable()
                .FirstOrDefault();

            if(result == null)
            {
                throw new Exception("Location not found!");
            }
            return result;
        }

        public async Task<IEnumerable<Location>> GetAllLocation()
        {
            return await _context.Locations.ToListAsync();
        }
    }
}
