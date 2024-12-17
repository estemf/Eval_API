using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe
{
    public class Badge
    {
        public int Id { get; set; } // Identifiant unique du badge
        public string Name { get; set; } // Nom du badge
        public string Description { get; set; } // Description du badge
        public string Criteria { get; set; } // Critères pour débloquer le badge
        public string ImageUrl { get; set; } // URL de l'image du badge
    }

}
