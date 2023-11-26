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
            var fb = new FactBase();
            
            var fol = new InferenceEngine();
              var alpha = new Predicate("Criminal", new List<object> { "West" });
              fb.AddFact(new Predicate("Owns", new List<object> { "Nono", "M1" }));
              fb.AddFact(new Predicate("Missile", new List<object> { "M1" }));
              fb.AddFact(new Predicate("American", new List<object> { "West" }));
              fb.AddFact(new Predicate("Enemy", new List<object> { "Nono", "America" }));
              Variable x, y, z;
              x = new Variable("X");
              y = new Variable("Y");
              z = new Variable("Z");


              Clause c1 = new Clause();
              c1.Addp(new Predicate("American", new List<object> { x }));
              c1.Addp(new Predicate("Weapon", new List<object> { y }));
              c1.Addp(new Predicate("Sells", new List<object> { x,y,z }));
              c1.Addp(new Predicate("Hostile", new List<object> { z }));
              c1.Setq(new Predicate("Criminal", new List<object> { x }));
              Clause c2 = new Clause();
              c2.Addp(new Predicate("Missile", new List<object> { x }));
              c2.Addp(new Predicate("Owns", new List<object> { "Nono", x }));
              c2.Setq(new Predicate("Sells", new List<object> { "West", x, "Nono" }));
              Clause c3 = new Clause();
              c3.Addp(new Predicate("Missile", new List<object> { x }));
              c3.Setq(new Predicate("Weapon", new List<object> { x }));
              Clause c4 = new Clause();
              c4.Addp(new Predicate("Enemy", new List<object> { x, "America" }));
              c4.Setq(new Predicate("Hostile", new List<object> { x }));
              fb.AddRule(c3);
              fb.AddRule(c1);
              fb.AddRule(c2);

              fb.AddRule(c4);

              Variable aa = new Variable("AA");
              //Console.WriteLine(fol.Unify(new Predicate("Missile", new List<object> { "AA" }), new Predicate("Missile", new List<object> { aa }), new Dictionary<string, string>()).Count);
            
         /*   var alpha = new Predicate("Evil", new List<object> { "John" });
            fb.AddFact(new Predicate("King", new List<object> { "John" }));
            fb.AddFact(new Predicate("Greedy", new List<object> { "John" }));
            Variable x = new Variable("x");
            Clause c1 = new Clause();
            c1.Addp(new Predicate("King", new List<object> { x }));
            c1.Addp(new Predicate("Greedy", new List<object> { x }));
            c1.Setq(new Predicate("Evil", new List<object> { x }));

            fb.AddRule(c1);*/
            Console.WriteLine("Fapte:");
            foreach(var f in fb.Facts)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine("\nReguli:");
            foreach (var f in fb.Rules)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine();
            // Propozitie atomica pe care dorim sa o verificam
            
            // Apelam algoritmul FOL-FC-ASK
            var substitution =fol.FOL_FC_ASK(fb,alpha);
                  // Verificam rezultatul
            if (substitution != null)
             {
                   Console.WriteLine("Propozitia este satisfiabila.");
             }
            else
            {
                Console.WriteLine("Propozitia nu este satisfiabila.");
            }
            Console.Read();
        }
    }
}
