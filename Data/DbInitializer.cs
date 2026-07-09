using DrKchida.Models;
using System.Collections.Generic;
using System.Linq;

namespace DrKchida.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // context.Database.EnsureDeleted(); // REMOVED: This was wiping the DB on every restart
            context.Database.EnsureCreated();

            // Look for any categories.
            if (context.ServiceCategories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new List<ServiceCategory>
            {
                new ServiceCategory
                {
                    Title = "Art des Injectables",
                    Subtitle = "Injectables de précision pour le rajeunissement facial",
                    CardLabel = "Injectables",
                    Slug = "injectables",
                    CardDescription = "Neuromodulateurs et Fillers de précision pour restaurer les volumes et lisser les traits.",
                    Description = "Injectables de précision pour le rajeunissement facial et la correction des rides.",
                    ImageUrl = "/images/beauty_procedure_mesotherapy.png",
                    AllowedSubCategories = "Botox (Neuromodulators);Fillers Avancés",
                    Treatments = new List<Treatment>
                    {
                        new Treatment { Name = "Haut du Visage & Contouring", SubCategory = "Botox (Neuromodulators)", ShowOnCard = true, Icon = "💉", TargetArea = "face" },
                        new Treatment { Name = "Meso-Botox (Visage & Cou)", SubCategory = "Botox (Neuromodulators)", Icon = "✨", TargetArea = "face" },
                        new Treatment { Name = "Sourire Gingival & Lip Flip", SubCategory = "Botox (Neuromodulators)", Icon = "👄", TargetArea = "face" },
                        new Treatment { Name = "Hyperhidrose (Transpiration Excessive)", SubCategory = "Botox (Neuromodulators)", Icon = "💧", TargetArea = "body" },
                        new Treatment { Name = "Rosacée & Raffinement des Pores", SubCategory = "Botox (Neuromodulators)", Icon = "🌸", TargetArea = "face" },
                        new Treatment { Name = "Rhinoplastie Médicale", SubCategory = "Fillers Avancés", ShowOnCard = true, Icon = "👃", TargetArea = "face" },
                        new Treatment { Name = "Techniques des Lèvres Russes & Françaises", SubCategory = "Fillers Avancés", ShowOnCard = true, Icon = "👄", TargetArea = "face" },
                        new Treatment { Name = "Cernes (Cernes Noirs) & Tempes", SubCategory = "Fillers Avancés", ShowOnCard = true, Icon = "👀", TargetArea = "face" },
                        new Treatment { Name = "Aqua Lift (Bio-remodelage)", SubCategory = "Fillers Avancés", Icon = "🌊", TargetArea = "face" },
                        new Treatment { Name = "Cernes", SubCategory = "Fillers Avancés", Icon = "👁️", TargetArea = "face" },
                        new Treatment { Name = "comblement de tempes", SubCategory = "Fillers Avancés", Icon = "✨", TargetArea = "face" }
                    }
                },
                new ServiceCategory
                {
                    Title = "Biostimulation",
                    Subtitle = "Structure & Élasticité",
                    CardLabel = "Biostimulation",
                    Slug = "biostimulation",
                    CardDescription = "Inducteurs de collagène et fils tenseurs pour un lifting naturel et durable.",
                    Description = "Traitements stimulants du collagène qui restaurent la structure et l'élasticité de la peau de l'intérieur.",
                    ImageUrl = "/images/anti_aging_results_hero.png",
                    AllowedSubCategories = "Radiesse & PLLA & PCL;Matériaux de Fils Résorbables (PDO,PCL,PLLA)",
                    Treatments = new List<Treatment>
                    {
                        new Treatment { Name = "Volumisation Visage & Cou", SubCategory = "Radiesse & PLLA & PCL", ShowOnCard = true, Icon = "🧬", TargetArea = "face" },
                        new Treatment { Name = "Rajeunissement du Décolleté", SubCategory = "Radiesse & PLLA & PCL", Icon = "✨", TargetArea = "body" },
                        new Treatment { Name = "Contouring Corporel Structurel", SubCategory = "Radiesse & PLLA & PCL", Icon = "🕺", TargetArea = "body" },
                        new Treatment { Name = "Fils Simple", SubCategory = "Matériaux de Fils Résorbables (PDO,PCL,PLLA)", Icon = "🧵", TargetArea = "face" },
                        new Treatment { Name = "Double Needle", SubCategory = "Matériaux de Fils Résorbables (PDO,PCL,PLLA)", Icon = "📐", TargetArea = "face" },
                        new Treatment { Name = "Lifting Instantané Visage & Cou", SubCategory = "Matériaux de Fils Résorbables (PDO,PCL,PLLA)", ShowOnCard = true, Icon = "✨", TargetArea = "face" },
                        new Treatment { Name = "Raffermissement des Tissus", SubCategory = "Matériaux de Fils Résorbables (PDO,PCL,PLLA)", ShowOnCard = true, Icon = "🧵", TargetArea = "body" },
                        new Treatment { Name = "Amélioration de la Qualité de Peau", SubCategory = "Matériaux de Fils Résorbables (PDO,PCL,PLLA)", Icon = "✨", TargetArea = "face body" }
                    }
                },
                new ServiceCategory
                {
                    Title = "Soins Régénératifs",
                    Subtitle = "Cellules & Facteurs de Croissance",
                    CardLabel = "Soins Régénératifs",
                    Slug = "regenerative",
                    CardDescription = "Cellules souches, Nano Fat et Exosomes pour une revitalisation profonde.",
                    Description = "Exploiter le pouvoir des cellules souches et des facteurs de croissance pour une guérison et une revitalisation profondes.",
                    ImageUrl = "/images/treatment_room.png",
                    AllowedSubCategories = "Nano Fat & Stem Cells;Exosomes & PDRN",
                    Treatments = new List<Treatment>
                    {
                        new Treatment { Name = "Traitement de l'Alopécie (Perte de Cheveux)", SubCategory = "Nano Fat & Stem Cells", ShowOnCard = true, Icon = "🧫", TargetArea = "hair" },
                        new Treatment { Name = "Révision de Cicatrices (Acné, traumatisme, chirurgie, brûlure)", SubCategory = "Nano Fat & Stem Cells", Icon = "🩹", TargetArea = "face body" },
                        new Treatment { Name = "Solutions pour Pathologies Cutanées", SubCategory = "Nano Fat & Stem Cells", Icon = "🧬", TargetArea = "face body" },
                        new Treatment { Name = "Rajeunissement Génital (externe, interne)", SubCategory = "Nano Fat & Stem Cells", Icon = "🌸", TargetArea = "body" },
                        new Treatment { Name = "Vergetures", SubCategory = "Exosomes & PDRN", Icon = "🩹", TargetArea = "body" },
                        new Treatment { Name = "Qualité de Peau Avancée & PRP", SubCategory = "Exosomes & PDRN", ShowOnCard = true, Icon = "🧪", TargetArea = "face hair" },
                        new Treatment { Name = "Revitalisation Visage & Mains", SubCategory = "Exosomes & PDRN", ShowOnCard = true, Icon = "👋", TargetArea = "face body" }
                    }
                },
                new ServiceCategory
                {
                    Title = "Technologie Avancée",
                    Subtitle = "Correction & Puissance",
                    CardLabel = "Lasers & Tech",
                    Slug = "technologie",
                    CardDescription = "Lasers médicaux et HIFU pour des résultats cliniques précis et sécurisés.",
                    Description = "Appareils énergétiques de pointe et lasers médicaux pour des corrections esthétiques non-invasives.",
                    ImageUrl = "/images/clinic_interior_luxury.png",
                    AllowedSubCategories = "Lasers Médicaux;HIFU & Énergie",
                    Treatments = new List<Treatment>
                    {
                        new Treatment { Name = "Laser Fractionné CO2", SubCategory = "Lasers Médicaux", ShowOnCard = true, Icon = "🔦", TargetArea = "face body laser" },
                        new Treatment { Name = "Épilation Laser", SubCategory = "Lasers Médicaux", Icon = "💎", TargetArea = "body face laser" },
                        new Treatment { Name = "Suppression de Tatouages (Détatouage)", SubCategory = "Lasers Médicaux", Icon = "✨", TargetArea = "body laser" },
                        new Treatment { Name = "Lifting Non-Chirurgical Visage & Cou", SubCategory = "HIFU & Énergie", ShowOnCard = true, Icon = "⚡", TargetArea = "face" },
                        new Treatment { Name = "Rajeunissement Vaginal", SubCategory = "HIFU & Énergie", ShowOnCard = true, Icon = "🌸", TargetArea = "body" },
                        new Treatment { Name = "Microneedling Médical", SubCategory = "HIFU & Énergie", Icon = "💉", TargetArea = "face body" }
                    }
                }
            };

            context.ServiceCategories.AddRange(categories);
            
            var galleryItems = new List<GalleryItem>
            {
                new GalleryItem { Category = "Face", Title = "Protocole 01", Description = "Restauration du volume du milieu du visage.", ImageBefore = "images/gallery/default-before.jpg", ImageAfter = "images/gallery/default-after.jpg" }
            };
            context.GalleryItems.AddRange(galleryItems);

            var videoItems = new List<VideoItem>
            {
                new VideoItem { Category = "Focus", Title = "Rajeunissement Facial", VideoUrl = "https://www.youtube.com/embed/9XInR7q5iOk" }
            };
            context.VideoItems.AddRange(videoItems);

            context.SaveChanges();
        }
    }
}
