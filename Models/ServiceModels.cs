using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DrKchida.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        
        [Required]
        public string? Title { get; set; } // Page Title
        
        public string? Subtitle { get; set; } // Page Subtitle
        
        public string? CardLabel { get; set; } // Badge on the card (e.g. "Visage")
        
        public string? Description { get; set; } // Long description for details page
        
        public string? CardDescription { get; set; } // Short description for services card
        
        public string? ImageUrl { get; set; }
        
        public string? Slug { get; set; } 

        public string? AllowedSubCategories { get; set; } // Semicolon separated list e.g. "Botox;Fillers"

        public List<Treatment>? Treatments { get; set; } = new();
    }

    public class Treatment
    {
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? Icon { get; set; }
        public string? ImageUrl { get; set; }
        public string? SubCategory { get; set; }


        public bool ShowOnCard { get; set; } = false; // To mark which ones appear in the 3-item list on the services page
        public string? TargetArea { get; set; } // e.g. "face", "body", "hair" or "face body"

        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }

        public string? FullDescription { get; set; } // For the "À Propos" section
        public string? Benefits { get; set; } // Format: "Title: Description" per line
        public string? ProcedureSteps { get; set; } // Format: "Title: Description" per line
    }
}
