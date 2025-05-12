using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Location;
using TourismWeb.Models;

namespace TourismWeb.Services.Interfaces
{
    public interface ILocationService
    {
        Task<LocationDTO> CreateLocation(LocationCreateDTO createDTO);
        Task<IEnumerable<LocationDTO>> GetAllLocation();
    }
}
