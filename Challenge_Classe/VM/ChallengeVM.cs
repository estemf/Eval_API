using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.VM
{
    public class ChallengeVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<QuestionVM> Questions { get; set; }
    }
}
