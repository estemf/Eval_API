using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.VM
{
    public class QuestionVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int IdType { get; set; }
        public string TypeName { get; set; }
        public ICollection<QuestionOptionVM> Options { get; set; }
    }
}
