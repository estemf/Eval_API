using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.Relation
{
    public class UserTeam
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}
