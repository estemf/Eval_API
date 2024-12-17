using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.Relation
{
    public class UserChallenge
    {
        public int Id { get; set; } // Identifiant unique
        public int UserId { get; set; } // Référence à l'utilisateur
        public int ChallengeId { get; set; } // Référence au challenge
        public string Status { get; set; } // Statut de participation (en cours, terminé, échoué)
        public int Score { get; set; } // Points gagnés
        public DateTime? CompletedAt { get; set; } // Date de finalisation (nullable)
        public string EvidenceUrl { get; set; } // URL de la preuve soumise (facultatif)
    }

}
