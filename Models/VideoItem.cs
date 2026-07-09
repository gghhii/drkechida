using System.ComponentModel.DataAnnotations;

namespace DrKchida.Models
{
    public class VideoItem
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = "";
        
        public string Description { get; set; } = "";
        
        public string VideoUrl { get; set; } = ""; // YouTube embed URL or Local File Path
        
        public string Category { get; set; } = "Focus Intervention";
        
        public string ThumbnailUrl { get; set; } = "";
    }
}
