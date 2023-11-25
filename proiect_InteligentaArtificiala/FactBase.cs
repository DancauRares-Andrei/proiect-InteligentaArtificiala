using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class FactBase
    {
        public List<Clause> Rules { get; set; }
        public List<Predicate> Facts { get; set; }
        public FactBase()
        {
            Rules = new List<Clause>();
            Facts = new List<Predicate>();
        }

        public void AddRule(Clause clause)
        {
            Rules.Add(clause);
        }
        public void AddFact(Predicate predicate)
        {
            Facts.Add(predicate);
        }
    }
}