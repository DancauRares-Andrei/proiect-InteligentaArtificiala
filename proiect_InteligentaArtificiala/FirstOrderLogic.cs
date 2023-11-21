using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class FirstOrderLogic
    {
        private List<object> usedVariableNames;

        public List<Clause> KnowledgeBase { get; set; }

        public FirstOrderLogic()
        {
            KnowledgeBase = new List<Clause>();
            usedVariableNames = new List<object>();
        }

        public Substitution FOL_FC_ASK(List<Clause> kb, Predicate alpha)
        {
            var newClauses = new List<Clause> {};

            do
            {
                var newClausesCopy = new List<Clause>(newClauses);
                newClauses.Clear();
                foreach (var r in kb)
                {                   
                    var (standardizedApartR, substitution) = StandardizeApart(r);

                    foreach (var theta in GenerateSubstitutions(standardizedApartR, kb))
                    {
                        var qPrime = Substitute(theta, standardizedApartR);
                        Console.WriteLine(qPrime.stringify());
                        if (!kb.Any(existing => existing.stringify()==qPrime.stringify()) && !newClausesCopy.Any(existing => existing.stringify() == qPrime.stringify()))
                        {
                            newClauses.Add(qPrime);
                            var phi = Unify(qPrime, alpha);

                            if (phi != null)
                                return phi;
                        }
                    }
                }

                kb.AddRange(newClausesCopy);
            } while (newClauses.Count != 0);

            return null;
        }



        private Tuple<Clause, Substitution> StandardizeApart(Clause clause)
        {
            throw new NotImplementedException();
        }




        private IEnumerable<Substitution> GenerateSubstitutions(Clause clause, List<Clause> kb)
        {
            throw new NotImplementedException();
        }

        

        private Clause Substitute(Substitution substitution, Clause clause)
        {
            throw new NotImplementedException();
        }

       

        private Substitution Unify(Clause c, Predicate a)
        {
            throw new NotImplementedException();
        }
    }
}
