using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Challenge_Classe.Relation;

namespace Challenge_Classe
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }

        // Référence au rôle
        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Référence au Team
        public ICollection<UserTeam> UserTeams { get; set; }
    }
}
