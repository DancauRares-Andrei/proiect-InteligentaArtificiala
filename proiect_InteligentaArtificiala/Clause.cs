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
        public List<Predicate> ps { get; set; }
        public Predicate q { get; set; }
        public Clause()
        {
            ps = new List<Predicate>();
            q = new Predicate();
        }

        public void Addp(Predicate predicate)
        {
            ps.Add(predicate);
        }

        public void Setq(Predicate predicate)
        {
            q = predicate;
        }

        public string stringify()
        {
            var predicateStrings = ps.Select((p, index) =>
            {
                // Nu afișa operatorul pentru primul predicat sau pentru un singur predicat
                if (index == 0 || ps.Count == 1)
                {
                    return p.stringify();
                }

                return $"AND {p.stringify()}";
            });
            string ret = string.Join(" ", predicateStrings);
            ret += " => " + q.stringify();
            return ret;
        }
    }
}
