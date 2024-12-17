using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.Relation
{
    public class QuestionOption
    {
        public int Id { get; set; }
        public string Text { get; set; } // Texte de l'option
        public bool IsCorrect { get; set; } // Indique si c'est la bonne réponse
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
