using System.ComponentModel.DataAnnotations;
using TourismWeb.Models;

namespace TourismWeb.DTOs.Image
{
    public class ImageDTO
    {
        public string Id { get; set; }
        public string Tour_Id { get; set; }
        public string Url { get; set; }

    }
}
