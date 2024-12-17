using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.Relation
{
    public class ChallengeQuestion
    {
        public int ChallengeId { get; set; }
        public Chall Challenge { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }

}
