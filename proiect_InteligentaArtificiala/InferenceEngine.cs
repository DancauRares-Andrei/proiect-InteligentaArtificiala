using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
     public class InferenceEngine
     {
        public Dictionary<string, string> FOL_FC_ASK(FactBase KB, Predicate alpha)
        {
            var substitutions = new Dictionary<string, string>();
            var newSentences = new List<Clause>();

            do
            {
                newSentences.Clear();

                foreach (Clause r in KB.Clauses)
                {
                    Console.WriteLine(StandardizeApart(r).stringify());
                }

                KB.Clauses.AddRange(newSentences);

            } while (newSentences.Count > 0);

            return null;
        }
        //Standardize Apart: Pentru fiecare clauza da nume diferite de variabile
        public Clause StandardizeApart(Clause r)
        {
            var renamedClause = new Clause();
            // Prelucrează fiecare predicat din clauză, schimbând numele variabilelor
            foreach (var predicateTuple in r.ps)
            {
                var standardizedPredicate = StandardizeApartPredicate(predicateTuple.Item1);
                renamedClause.Addp(standardizedPredicate, predicateTuple.Item2);
            }

            if (r.q != null)
            {
                var standardizedQ = StandardizeApartPredicate(r.q.Item1);
                renamedClause.Setq(standardizedQ, r.q.Item2);
            }
            return renamedClause;
        }
        private Predicate StandardizeApartPredicate(Predicate predicate)
        {
            var variableCounter = 1;
            var variableNameMapping = new Dictionary<string, string>();

            var standardizedArguments = predicate.Arguments.Select(arg =>
            {
                if (arg is Variable var)
                {
                    if (!variableNameMapping.TryGetValue(var.Name, out var standardizedName))
                    {
                        standardizedName = "X" + variableCounter++;
                        variableNameMapping[var.Name] = standardizedName;
                    }
                    return new Variable(standardizedName);
                }

                return arg;
            }).ToList();

            return new Predicate(predicate.Name, standardizedArguments);
        }
        public bool IsRenaming(Clause q,List<Clause> a)
        {
            return true;
        }
    }
}
