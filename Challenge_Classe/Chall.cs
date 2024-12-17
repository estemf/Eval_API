using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Challenge_Classe.Relation;

namespace Challenge_Classe
{
    public class Chall
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageB64 { get; set; }
        public int Points { get; set; }
        public string Category { get; set; }
        public string DifficultyLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GuideUrl { get; set; }

        public ICollection<ChallengeQuestion> ChallengeQuestions { get; set; }
        public ICollection<TeamChallenge> TeamChallenges { get; set; }
    }
}
