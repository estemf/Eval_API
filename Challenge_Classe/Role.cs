using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } // Exemples : "User", "Admin"

        // Navigation property
        public ICollection<User> Users { get; set; }
    }
}
