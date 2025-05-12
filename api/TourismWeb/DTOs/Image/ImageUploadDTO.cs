namespace TourismWeb.DTOs.Image
{
    public class ImageUploadDTO
    {
        public string TourId { get; set; }
        public IFormFile File { get; set; }
    }
}
