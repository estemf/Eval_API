using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Challenge_Classe.Relation;

namespace Challenge_Classe
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; } // Contenu de la question
        public int IdType { get; set; } // Référence à un type de question
        public QuestionType Type { get; set; } // Navigation vers le type
        public ICollection<QuestionOption> Options { get; set; } // Options possibles pour la question
        public ICollection<ChallengeQuestion> ChallengeQuestions { get; set; } // Relation avec Challenge
    }

}
