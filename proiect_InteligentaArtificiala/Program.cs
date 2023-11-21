using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    class Program
    {
        static void Main(string[] args)
        {
            var fol = new FirstOrderLogic();

            var kb = new List<Clause>
        {
               new Clause().AddPredicate(new Predicate("American", new List<object> {"x"}), false),
            new Clause().AddPredicate(new Predicate("Weapon", new List<object> {"y"}), false),
            new Clause().AddPredicate(new Predicate("Sells", new List<object> {"x", "y", "z"}), false),
            new Clause().AddPredicate(new Predicate("Hostile", new List<object> {"z"}), false),
            new Clause().AddPredicate(new Predicate("Criminal", new List<object> {"x"}), true),

            new Clause().AddPredicate(new Predicate("Owns", new List<object> {"Nono", "M1"}), false),
            new Clause().AddPredicate(new Predicate("Missile", new List<object> {"M1"}), false),
            new Clause().AddPredicate(new Predicate("Missile", new List<object> {"x"}), false),
            new Clause().AddPredicate(new Predicate("Owns", new List<object> {"Nono", "x"}), false),
            new Clause().AddPredicate(new Predicate("Sells", new List<object> {"West", "x", "Nono"}), true),
            new Clause().AddPredicate(new Predicate("Missile", new List<object> {"x"}), true),
            new Clause().AddPredicate(new Predicate("Weapon", new List<object> {"x"}), false),
            new Clause().AddPredicate(new Predicate("Enemy", new List<object> {"x", "America"}), false),
            new Clause().AddPredicate(new Predicate("Hostile", new List<object> {"x"}), true),
            new Clause().AddPredicate(new Predicate("American", new List<object> {"West"}), false),
            new Clause().AddPredicate(new Predicate("Enemy", new List<object> {"Nono", "America"}), false)
        };
            // Propozitie atomica pe care dorim sa o verificam
        var alpha = new Predicate("Criminal", new List<object> { "West" });

            // Apelam algoritmul FOL-FC-ASK
            var substitution = fol.FOL_FC_ASK(kb, alpha);
            
            // Verificam rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia este satisfiabila.");
                Console.WriteLine("Substitutie:");
                foreach (var entry in substitution.Values)
                {
                    Console.WriteLine($"{entry.Key} -> {entry.Value}");
                }
            }
            else
            {
                Console.WriteLine("Propozitia nu este satisfiabila.");
            }
        Console.Read();
        }
    }
}
