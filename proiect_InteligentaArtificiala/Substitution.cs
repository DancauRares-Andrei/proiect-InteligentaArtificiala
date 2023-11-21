using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class Substitution
    {
        public Dictionary<string, object> Values { get; set; }

        public Substitution()
        {
            Values = new Dictionary<string, object>();
        }
        public Substitution(Substitution substitution)
        {
            Values = substitution.Values;
        }
    }
}
