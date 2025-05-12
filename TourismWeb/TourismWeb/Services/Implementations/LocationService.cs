using AutoMapper;
using TourismWeb.DTOs.Location;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<LocationDTO> CreateLocation(LocationCreateDTO createDTO)
        {
            var location = _mapper.Map<Location>(createDTO);
            var createdLocation = await _locationRepository.CreateLocation(location);
            return _mapper.Map<LocationDTO>(createdLocation);
        }

        public async Task<IEnumerable<LocationDTO>> GetAllLocation()
        {
            var result = await _locationRepository.GetAllLocation();
            return _mapper.Map<IEnumerable<LocationDTO>>(result);
        }
    }
}
