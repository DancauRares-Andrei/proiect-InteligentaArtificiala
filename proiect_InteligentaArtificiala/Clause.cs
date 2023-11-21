using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class Clause
    {
        public List<Tuple<Predicate, bool>> Predicates { get; set; }

        public Clause()
        {
            Predicates = new List<Tuple<Predicate, bool>>();
        }

        public Clause AddPredicate(Predicate predicate, bool isNegated = false)
        {
            Predicates.Add(new Tuple<Predicate, bool>(predicate, isNegated));
            return this;
        }

        public string stringify()
        {
            var predicateStrings = Predicates.Select(p => p.Item2 ? $"~{p.Item1.stringify()}" : p.Item1.stringify());
            return string.Join(" && ", predicateStrings);
        }
    }
}
