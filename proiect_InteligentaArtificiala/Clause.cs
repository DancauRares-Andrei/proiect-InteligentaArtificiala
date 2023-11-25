using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class Clause
    {
        //Clauza va fi formata din doua parti, ce se afla in dreapta si ce se afla in stanga implicatiei
        public List<Tuple<Predicate, bool>> ps { get; set; }
        public Tuple<Predicate, bool> q { get; set; }
        public Clause()
        {
            ps = new List<Tuple<Predicate, bool>>();
            q = null;
        }

        public void Addp(Predicate predicate, bool isNegated = false)
        {
            ps.Add(new Tuple<Predicate, bool>(predicate, isNegated));
        }

        public void Setq(Predicate predicate, bool isNegated = false)
        {
            q = new Tuple<Predicate, bool>(predicate, isNegated);
        }

        public string stringify()
        {
            var predicateStrings = ps.Select((p, index) =>
            {
                // Nu afișa operatorul pentru primul predicat sau pentru un singur predicat
                if (index == 0 || ps.Count == 1)
                {
                    return p.Item2 ? $"NOT {p.Item1.stringify()}" : p.Item1.stringify();
                }

                return p.Item2 ? $"AND NOT {p.Item1.stringify()}" : $"AND {p.Item1.stringify()}";
            });
            string ret = string.Join(" ", predicateStrings);
            if (q != null)
                if (q.Item2)
                    ret += " => NOT " + q.Item1.stringify();
                else
                    ret += " => " + q.Item1.stringify();
            return ret;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Clause other = (Clause)obj;

            return ps.SequenceEqual(other.ps) && q==other.q;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var predicateTuple in ps)
                {
                    hash = hash * 23 + predicateTuple.Item1.GetHashCode();
                    hash = hash * 23 + predicateTuple.Item2.GetHashCode();
                }
                hash = hash * 23 + this.q.Item1.GetHashCode();
                hash = hash * 23 + this.q.Item2.GetHashCode();
                return hash;
            }
        }
    }
}
