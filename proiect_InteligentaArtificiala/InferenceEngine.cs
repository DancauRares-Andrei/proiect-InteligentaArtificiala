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
            var newSentences = new List<Clause>();
            KB = StandardizeApartFactBase(KB);
            var substitutionsList = GenerateSubstitutionsFromFacts(StandardizeApartFactBase(KB));
            /*foreach (var s in substitutionsList)
            {
                foreach (string key in s.Keys)
                {
                    s.TryGetValue(key,out string value);
                    Console.WriteLine($"{key}=>{value}");
                }
                    
            }*/
            do
            {
                newSentences.Clear();

                foreach (Clause r in KB.Clauses)
                {
                    Clause newClause = StandardizeApart(r);
                    Console.WriteLine(newClause.stringify());
                   /* foreach(var theta in substitutionsList)
                    {
                        if (true)//Egalitate intre substitutii
                        {
                            Predicate q_prim = SubstPredicate(theta, r.q.Item1);
                            Clause qp = new Clause();
                            qp.Addp(q_prim);
                            //qp.Setq(q_prim);
                            if(!IsRenaming(qp, newSentences) && !IsRenaming(qp, KB.Clauses))
                            {
                                Console.WriteLine(qp.stringify());
                                newSentences.Add(qp);
                            }
                            else
                            {
                                //Console.WriteLine(q_prim.stringify());
                            }
                        }
                    }*/

                }

                KB.Clauses.AddRange(newSentences);

            } while (newSentences.Count > 0);

            return null;
        }
        //Standardize Apart: Pentru fiecare clauza da nume diferite de variabile
        public FactBase StandardizeApartFactBase(FactBase fb)
        {
            FactBase factBase = new FactBase();
            foreach(Clause c in fb.Clauses)
            {
                factBase.AddClause(StandardizeApart(c));
            }
            return factBase;
        }
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
        public bool IsRenaming(Clause q, List<Clause> a)
        {
            foreach (var clause in a)
            {
                if (IsRenamingClause(q, clause))
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsRenamingClause(Clause q, Clause r)
        {
            if (q.q.Item1.Name != r.q.Item1.Name || q.q.Item2 != r.q.Item2)
                return false;
            if (q.ps.Count != r.ps.Count)
                return false;
            for (int i = 0; i < q.ps.Count; i++)
            {
                if (q.ps[i].Item1.Name != r.ps[i].Item1.Name || q.ps[i].Item2 != r.ps[i].Item2)
                    return false;
            }
            return true;
        }
        public Clause Subst(Dictionary<string, string> theta, Clause clause)
        {
            var substitutedClause = new Clause();

            foreach (var predicateTuple in clause.ps)
            {
                var substitutedPredicate = SubstPredicate(theta, predicateTuple.Item1);
                substitutedClause.Addp(substitutedPredicate, predicateTuple.Item2);
            }

            if (clause.q != null)
            {
                var substitutedQ = SubstPredicate(theta, clause.q.Item1);
                substitutedClause.Setq(substitutedQ, clause.q.Item2);
            }

            return substitutedClause;
        }
        private Predicate SubstPredicate(Dictionary<string, string> theta, Predicate predicate)
        {
            var substitutedArguments = predicate.Arguments.Select(arg => SubstArgument(theta, arg)).ToList();
            return new Predicate(predicate.Name, substitutedArguments);
        }
        private object SubstArgument(Dictionary<string, string> theta, object argument)
        {
            if (argument is Variable var && theta.TryGetValue(var.Name, out var substitution))
            {
                return new Variable(substitution);
            }

            return argument;
        }
        public List<Dictionary<string, string>> GenerateSubstitutionsFromFacts(FactBase factBase)
        {
            List<Dictionary<string, string>> substitutii_posibile = new List<Dictionary<string, string>>();
            List<string> neVar = new List<string>();
            List<string> varNames = new List<string>();
            foreach (Clause c in factBase.Clauses)
            {
                foreach (var tuple in c.ps)
                {
                    foreach (var a in tuple.Item1.Arguments)
                        if (!(a is Variable))
                        {
                            if (!neVar.Contains(a.ToString()))
                                neVar.Add(a.ToString());
                        }
                        else
                        {
                            if (!varNames.Contains((a as Variable).Name))
                                varNames.Add((a as Variable).Name);
                        }
                }
            }
            foreach(string varname in varNames)
            {
                foreach (string nevar in neVar)
                {
                    //
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(varname, nevar);
                    substitutii_posibile.Add(keyValues);
                }
                    
            }
            return substitutii_posibile;
        }
    }
       
 }
