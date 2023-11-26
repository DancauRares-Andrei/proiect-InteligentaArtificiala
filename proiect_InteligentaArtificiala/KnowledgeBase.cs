using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    //Clasa pentru baza de cunostinte, contine o lista de reguli si o lista de fapte
    //Faptele sunt propozitii atomice, iar regulile sunt clauze definite de ordinul intai.
    public class KnowledgeBase
    {
        public List<Clause> Rules { get; set; }
        public List<Predicate> Facts { get; set; }
        public KnowledgeBase()
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
        public void Clear()
        {
            Facts.Clear();
            Rules.Clear();
        }
    }
}