using Challenge_Classe.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe
{
    internal class ChallengeHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChallengeId { get; set; }
        public bool Result { get; set; } 
        public DateTime ParticipationDate { get; set; }

        public virtual User User { get; set; }
        public virtual Chall Challenge { get; set; }
    }
}
