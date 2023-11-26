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
        public Dictionary<string, string> FOL_FC_ASK(FactBase KB, Predicate alpha)
        {
            var newPredicates = new List<Predicate>();
            do {
                newPredicates.Clear();
                foreach (Clause r in KB.Rules)
                {
                    Clause standard = r;//StandardizeApart(r) nu este necesar pentru ca stabilesc de la inceput sa nu fie conflicte
                    List<Predicate> p1pn = standard.ps;
                    var substitutionsList = IdentifySubstitutions(KB.Facts, standard);//Determinare substitutii posibile=egalitate intre substitutii
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
            foreach(Predicate predicate in standard.ps)
            {
                if (!facts.Exists(x => x.Name == predicate.Name))
                    return new List<Dictionary<string, string>>();
                foreach (Predicate predicate1 in facts)
                {
                    if (predicate.Name == predicate1.Name)
                    {
                        var theta = Unify(predicate, predicate1, new Dictionary<string, string>());
                        if (theta == null)
                            return new List<Dictionary<string, string>>();
                        else
                            substitutionsList.Add(theta);
                    }                  
                }
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

        private Predicate SubstPredicate(Dictionary<string, string> theta, Predicate predicate)
        {
            var substitutedArguments = predicate.Arguments.Select(arg => SubstArgument(theta, arg)).ToList();
            return new Predicate(predicate.Name, substitutedArguments);
        }
        private object SubstArgument(Dictionary<string, string> theta, object argument)
        {
            if (argument is Variable var && theta.TryGetValue(var.Name, out var substitution))
            {
                return new Variable(substitution);//substitution dar strica refacerea; cu var.Name se vede, dar nu face procesul bine
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
            if (x == null)
                return null;
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
