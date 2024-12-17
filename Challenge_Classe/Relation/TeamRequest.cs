using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_Classe.Relation
{
    public class TeamRequest
    {
        public string Name { get; set; }
        public List<Guid> UserIds { get; set; }
    }
}
