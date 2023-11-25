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
            var newPredicates = new List<Predicate>();
            var substitutionsList = GenerateSubstitutionsFromFacts(StandardizeApartFactBase(KB));
            foreach (var s in substitutionsList)
            {
                foreach (string key in s.Keys)
                {
                    s.TryGetValue(key, out string value);
                    Console.WriteLine($"{key}=>{value}");
                }

            }
            do {
                newPredicates.Clear();
                foreach (Clause r in KB.Rules)
                {
                    Clause standard = StandardizeApart(r);
                    List<Predicate> p1pn = standard.ps;
                    foreach (var theta in substitutionsList)
                    {
                        if (true)//egalitate intre substitutii
                        {
                            Predicate q_prime = SubstPredicate(theta, standard.q);
                            if (!IsRenaming(q_prime, newPredicates) && !IsRenaming(q_prime, KB.Facts))
                            {
                                Console.WriteLine(q_prime.stringify());
                                newPredicates.Add(q_prime);
                                var phi = Unify(q_prime, alpha,new Dictionary<string, string>());
                                if (phi != null)
                                    return phi;
                            }
                        }
                    }
                }
                KB.Facts.AddRange(newPredicates);
            } while (newPredicates.Count > 0);
            return null;
        }
        public FactBase StandardizeApartFactBase(FactBase fb)
        {
            FactBase factBase = new FactBase();
            foreach (Clause c in fb.Rules)
            {
                factBase.AddRule(StandardizeApart(c));
            }
            foreach (Predicate p in fb.Facts)
            {
                factBase.AddFact(StandardizeApartPredicate(p));
            }
            return factBase;
        }
        public Clause StandardizeApart(Clause r)
        {
            var renamedClause = new Clause();
            // Prelucrează fiecare predicat din clauză, schimbând numele variabilelor
            foreach (var predicate in r.ps)
            {
                var standardizedPredicate = StandardizeApartPredicate(predicate);
                renamedClause.Addp(standardizedPredicate);
            }

            if (r.q != null)
            {
                var standardizedQ = StandardizeApartPredicate(r.q);
                renamedClause.Setq(standardizedQ);
            }
            return renamedClause;
        }
        public bool IsRenaming(Predicate predicate, List<Predicate> predicates)
        {
            foreach(Predicate predicate1 in predicates)
            {
                if (predicate1.Name == predicate.Name)
                    return true;
            }
            return false;
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
        public List<Predicate> Subst(Dictionary<string, string> theta, List<Predicate> predicates)
        {
            var substitutedClause = new List<Predicate>();

            foreach (var predicateTuple in predicates)
            {
                var substitutedPredicate = SubstPredicate(theta, predicateTuple);
                substitutedClause.Add(substitutedPredicate);
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
            foreach (Clause c in factBase.Rules)
            {
                foreach (var predicate in c.ps)
                {
                    foreach (var a in predicate.Arguments)
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
                foreach (var a in c.q.Arguments)
                {
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
            foreach (Predicate p in factBase.Facts)
            {
                foreach (var a in p.Arguments)
                {
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
            foreach (string varname in varNames)
            {
                foreach (string nevar in neVar)
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(varname, nevar);
                    substitutii_posibile.Add(keyValues);
                }

            }
            return substitutii_posibile;
        }
        public Dictionary<string,string> Unify(object x,object y,Dictionary<string,string> theta)
        {
            if (theta == null)
                return null;
            else if (x.Equals(y))
                return theta;
            else if (IsVariable(x))
                return UnifyVar(x as Variable, y, theta);
            else if (IsVariable(y))
                return UnifyVar(y as Variable, x, theta);
            else if (IsCompound(x) && IsCompound(y))
                return Unify((x as Predicate).Arguments, (y as Predicate).Arguments, Unify((x as Predicate).Name, (y as Predicate).Name, theta));
            else if (IsList(x) && IsList(y))
                return Unify((x as List<object>).Skip(1), (y as List<object>).Skip(1), Unify((x as List<object>).First(), (y as List<object>).First(), theta));
            else return null;
        }
        public Dictionary<string, string> UnifyVar(Variable v,object x,Dictionary<string,string> theta)
        {
            if(theta.TryGetValue(v.Name, out var val))
            {
                return Unify(val, x, theta);
            }
            if(theta.TryGetValue(x.ToString(),out var existingVal))
            {
                return Unify(v, val, theta);
            }
            else if (OccurCheck(v, x))
            {
                return null;
            }
            else
            {
                theta.Add(v.Name, x.ToString());
                return theta;
            }
        }
        private bool OccurCheck(Variable var,object x)
        {
            if (x is Predicate)
            {
                return (x as Predicate).Arguments.Contains(var);
            }
            else
            {
                return false;
            }
        }
        private bool IsVariable(object x)
        {
            return x is Variable;
        }
        private bool IsCompound(object x)
        {
            return x is Predicate;
        }
        private bool IsList(object x)
        {
            return x is List<object>;
        }
    }
}
