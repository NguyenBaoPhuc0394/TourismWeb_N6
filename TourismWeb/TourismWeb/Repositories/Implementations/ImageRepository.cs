using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class ImageRepository : IImageRepository
    {
        private readonly TourismDbContext _context;
        public ImageRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<Image> CreateImage(Image image)
        {
            var result = _context.Images.FromSqlRaw("Exec CreateImage @TourId = {0}, @Url = {1}",
                image.Tour_Id, image.Url).AsEnumerable().FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Tour not found!");
            }
            return result;
        }

        public async Task DeleteImageById(string id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                throw new KeyNotFoundException("Account not found!");
            }
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Image>> GetAllImages()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<Image> GetImageById(string id)
        {
            var result = await _context.Images.FindAsync(id);
            if (result == null)
            {
                throw new KeyNotFoundException("Tour not found!");
            }
            return result;
        }

        public async Task<IEnumerable<Image>> GetImagesByTourId(string tourId)
        {
            var result = await _context.Images
                .Where(img => img.Tour_Id == tourId)
                .ToListAsync();
            //if (result == null)
            //{
            //    throw new KeyNotFoundException("Tours not found!");
            //}
            return result;
        }

        public async Task<Image> UpdateImageById(string id, Image image)
        {
            var existingImage = await _context.Images.FindAsync(id);
            if (existingImage == null)
            {
                throw new KeyNotFoundException("Category not found!");
            }

            existingImage.Tour_Id = existingImage.Tour_Id;
            existingImage.Url = existingImage.Url;

            await _context.SaveChangesAsync();

            return existingImage;
        }
    }
}
