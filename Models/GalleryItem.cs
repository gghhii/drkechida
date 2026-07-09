using System.ComponentModel.DataAnnotations;

namespace DrKchida.Models
{
    public class GalleryItem
    {
        public int Id { get; set; }
        
        [Required]
        public string Category { get; set; } = "Face"; // Face, Hair, or Body
        
        [Required]
        public string Title { get; set; } = "";
        
        public string Description { get; set; } = "";
        
        // Separate images for before and after
        public string ImageBefore { get; set; } = "images/gallery/default-before.jpg";
        
        public string ImageAfter { get; set; } = "images/gallery/default-after.jpg";
        
        // Helper method to get display name for category
        public string GetCategoryDisplay()
        {
            return Category switch
            {
                "Face" => "Visage",
                "Hair" => "Cheveux",
                "Body" => "Corps",
                _ => Category
            };
        }
    }
}
