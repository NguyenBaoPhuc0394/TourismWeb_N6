using AutoMapper;
using TourismWeb.DTOs.Image;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        public ImageService(IImageRepository imageRepository, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ImageDTO> CreateImage(string tourId, IFormFile file)
        {
            try
            {
                var publicKey = await _cloudinaryService.UploadImage(file);
                if (publicKey == null) {
                    throw new ArgumentException("Upload anh that bai");
                }
                var result = await _imageRepository.CreateImage(tourId, publicKey);
                return _mapper.Map<ImageDTO>(result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> DeleteImageById(string id)
        {
            string publicKey = await _imageRepository.GetImageUrl(id);
            if (publicKey == null)
            {
                return false;
            }

            bool isDeletedFromCloud = await _cloudinaryService.DeleteImage(publicKey);
            if (!isDeletedFromCloud)
            {
                return false;
            }

            await _imageRepository.DeleteImageById(id);

            return true;
        }

        public async Task<IEnumerable<ImageDTO>> GetAllImages()
        {
            var images = await _imageRepository.GetAllImages();
            return _mapper.Map<IEnumerable<ImageDTO>>(images);
        }

        public async Task<ImageDTO> GetImageById(string id)
        {
            var image = await _imageRepository.GetImageById(id);
            return _mapper.Map<ImageDTO>(image);
        }

        public async Task<IEnumerable<ImageTourDTO>> GetImagesByTourId(string tourId)
        {
            var images = await _imageRepository.GetImagesByTourId(tourId);
            var result = new List<ImageTourDTO>();
            foreach (var img in images)
            {
                result.Add(new ImageTourDTO { URL = _cloudinaryService.ToUrlImage(img.Url) });
            }
            return result;
;        }

        public async Task<ImageDTO> UpdateImageById(string id, ImageDTO updatedImage)
        {
            var image = _mapper.Map<Models.Image>(updatedImage);
            var result = await _imageRepository.UpdateImageById(id, image);
            return _mapper.Map<ImageDTO>(result);
        }

        public async Task<IEnumerable<ImageDTO>> GetImagesDataByTourId(string tourId)
        {
            var images = await _imageRepository.GetImagesByTourId(tourId);
            var imagesDTO = _mapper.Map<IEnumerable<ImageDTO>>(images);

            foreach (var img in imagesDTO)
            {
                img.Url = _cloudinaryService.ToUrlImage(img.Url);
            }

            return imagesDTO;
        }


    }
}
