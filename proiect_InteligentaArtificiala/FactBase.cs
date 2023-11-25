using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class FactBase
    {
        public List<Clause> Clauses { get; set; }

        public FactBase()
        {
            Clauses = new List<Clause>();
        }

        public void AddClause(Clause clause)
        {
            Clauses.Add(clause);
        }
    }
}