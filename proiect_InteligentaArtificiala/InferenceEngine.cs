using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//De rezolvat problema StandardizeApart si generare Substitutii
namespace proiect_InteligentaArtificiala
{
    public class InferenceEngine
    {
        private int globalVariableCounter = 1;
        public Dictionary<string, string> FOL_FC_ASK(FactBase KB, Predicate alpha)
        {
            var newPredicates = new List<Predicate>();
            do {
                newPredicates.Clear();
                foreach (Clause r in KB.Rules)
                {
                    Clause standard = StandardizeApart(r);//StandardizeApart(r)
                    List<Predicate> p1pn = standard.ps;
                    var substitutionsList = IdentifySubstitutions(KB.Facts, standard);
                   /* foreach (var s in substitutionsList)
                    {
                        foreach (string key in s.Keys)
                        {
                            s.TryGetValue(key, out string value);
                            Console.WriteLine($"{key}=>{value}");
                        }
                   
                    }*/
                    //Determinare substitutii posibile=egalitate intre substitutii
                    foreach (var theta in substitutionsList)
                    {
                            Predicate q_prime = SubstPredicate(theta, standard.q);
                            if (!IsRenaming(q_prime, newPredicates) && !IsRenaming(q_prime, KB.Facts))
                            {
                              //Afisare propozitii noi la fiecare pas
                               Console.WriteLine(q_prime.stringify());
                                newPredicates.Add(q_prime);
                                var phi = Unify(q_prime, alpha,new Dictionary<string, string>());
                            if (phi != null)
                            {
                                Console.WriteLine(standard.stringify());
                                return phi;
                            }
                            }
                    }                  
                }
                KB.Facts.AddRange(newPredicates);
                Console.WriteLine("");
            } while (newPredicates.Count > 0);
            return null;
        }
        private List<Dictionary<string, string>> IdentifySubstitutions(List<Predicate> facts, Clause standard)
        {
            var substitutionsList = new List<Dictionary<string, string>>();
            var theta = new Dictionary<string, string>();
            foreach (var fact in facts)
            {
                // Realizează unificare între fapt și antecedentul regulii standardizate
                theta = Unify(fact, standard);
                if (theta != null)
                    substitutionsList.Add(theta);            
            }

            return substitutionsList;
        }
        public bool IsRenaming(Predicate predicate, List<Predicate> predicates)
        {
            foreach (Predicate predicate1 in predicates)
            {
                if (predicate1.Name == predicate.Name)
                    return true;
            }
            return false;
        }
        private Dictionary<string, string> Unify(Predicate fact, Clause standard)
        {
            // Implementează unificarea între fapt și antecedentul regulii standardizate
            foreach(Predicate predicate in standard.ps)
            {
                Dictionary<string, string> val = Unify(fact, predicate, new Dictionary<string, string>());
                if (val != null)
                    return val;
            }
            return Unify(fact, standard.q, new Dictionary<string, string>());
        }

        /*  public List<Predicate> Subst(Dictionary<string, string> theta, List<Predicate> predicates)
          {
              var substitutedClause = new List<Predicate>();

              foreach (var predicateTuple in predicates)
              {
                  var substitutedPredicate = SubstPredicate(theta, predicateTuple);
                  substitutedClause.Add(substitutedPredicate);
              }

              return substitutedClause;
          }*/
        public Clause StandardizeApart(Clause r)
        {
            var renamedClause = new Clause();
            var variableNameMapping = new Dictionary<string, string>(); // Dicționar pentru a ține evidența numelor variabilelor

            // Prelucrează fiecare predicat din clauză, schimbând numele variabilelor
            foreach (var predicate in r.ps)
            {
                var standardizedPredicate = StandardizeApartPredicate(predicate, variableNameMapping);
                renamedClause.Addp(standardizedPredicate);
            }

            if (r.q != null)
            {
                var standardizedQ = StandardizeApartPredicate(r.q, variableNameMapping);
                renamedClause.Setq(standardizedQ);
            }

            return renamedClause;
        }

        private Predicate StandardizeApartPredicate(Predicate predicate, Dictionary<string, string> variableNameMapping)
        {
            var standardizedArguments = predicate.Arguments.Select(arg =>
            {
                if (arg is Variable var)
                {
                    if (!variableNameMapping.TryGetValue(var.Name, out var standardizedName))
                    {
                        standardizedName = "X" + globalVariableCounter++;
                        variableNameMapping[var.Name] = standardizedName;
                    }
                    return new Variable(standardizedName);
                }

                return arg;
            }).ToList();

            return new Predicate(predicate.Name, standardizedArguments);
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
                return new Variable(var.Name);//substitution
            }

            return argument;
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
            {
                /*if ((x as List<object>).Count == 0 && (y as List<object>).Count == 0)
                    return theta;
                else if ((x as List<object>).Count == 0 || (y as List<object>).Count == 0)
                    return null;
                object a = (x as List<object>).First();
                object b = (y as List<object>).First();
                (x as List<object>).RemoveAt(0);
                (y as List<object>).RemoveAt(0);
               if ((x as List<object>).Count == 0 && (y as List<object>).Count == 0)
                    return null;                    
                return Unify(x,y , Unify(a, b, theta));*/
                return UnifyList(x as List<object>, y as List<object>, theta);
            }
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
        private Dictionary<string, string> UnifyList(List<object> x, List<object> y, Dictionary<string, string> theta)
        {
            if (x.Count == 0 && y.Count == 0)
            {
                return theta; // Liste goale se unifică direct
            }
            else if (x.Count > 0 && y.Count > 0)
            {
                // Unifică primul element al fiecărei liste
                var unifiedFirst = Unify(x.First(), y.First(), theta);

                if (unifiedFirst != null)
                {
                    // Unifică restul listelor recursiv
                    return UnifyList(x.Skip(1).ToList(), y.Skip(1).ToList(), unifiedFirst);
                }
            }

            return null; // Returnează null dacă nu se poate realiza unificarea
        }

    }
}
