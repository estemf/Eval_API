using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.Relation
{
    public class TeamChallenge
    {
        public Guid Id { get; set; } // Clé primaire pour cette table
        public Guid TeamId { get; set; } // Clé étrangère correspondant à Team.Id
        public Team Team { get; set; }
        public int ChallengeId { get; set; } // Clé étrangère correspondant à Chall.Id
        public Chall Challenge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
