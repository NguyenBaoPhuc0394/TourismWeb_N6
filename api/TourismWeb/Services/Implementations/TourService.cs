using AutoMapper;
using TourismWeb.DTOs.Tour;
using TourismWeb.DTOs.Tours;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public TourService(ITourRepository tourRepository, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<TourDTO> CreateTour(TourCreateDTO tourDTO)
        {
            try
            {
                var createdTour = await _tourRepository.CreateTour(tourDTO);
                return _mapper.Map<TourDTO>(createdTour);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteTourById(string id)
        {
            await _tourRepository.DeleteTourById(id);
        }

        public async Task<IEnumerable<TourDTO>> GetAllTours()
        {
            var tours = await _tourRepository.GetAllTours();
            return _mapper.Map<IEnumerable<TourDTO>>(tours);
        }

        public async Task<TourDTO> GetTourById(string id)
        {
            var tour = await _tourRepository.GetTourById(id);
            return _mapper.Map<TourDTO>(tour);
        }

        public async Task<IEnumerable<TourDTO>> GetToursByCategory(string categoryId)
        {
            var tours = await _tourRepository.GetToursByCategory(categoryId);
            return _mapper.Map<IEnumerable<TourDTO>>(tours);
        }

        public async Task<TourCreateDTO> UpdateTourById(string id, TourUpdateDTO tour)
        {
            var tourResult = await _tourRepository.UpdateTourById(id, tour);
            return _mapper.Map<TourCreateDTO>(tourResult);
        }

        public async Task<IEnumerable<TourCardDTO>> GetAllPromotionTours()
        {
            var result = await _tourRepository.GetAllPromotionTours();
            foreach (var tour in result)
            {
                if (!string.IsNullOrEmpty(tour.ImageUrl))
                {
                    tour.ImageUrl = _cloudinaryService.ToUrlImage(tour.ImageUrl);
                }
            }
            return result;
        }

        public async Task<IEnumerable<TourCardDTO>> SearchTours(TourSearchDTO searchData)
        {
            var result = await _tourRepository.SearchTours( searchData );
            foreach (var tour in result)
            {
                if (!string.IsNullOrEmpty(tour.ImageUrl))
                {
                    tour.ImageUrl = _cloudinaryService.ToUrlImage(tour.ImageUrl);
                }
            }
            return result;
        }

        public async Task<IEnumerable<TourTableDTO>> GetAllTourTable()
        {
            var result = await _tourRepository.GetAllTours();
            return _mapper.Map<IEnumerable<TourTableDTO>>(result);
        }
    }
}
