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
            
            Variable x, y, z,Nono,America,M1,West;
            x = new Variable("X");
            y = new Variable("Y");
            z = new Variable("Z");
            Nono = new Variable("Nono");
            America = new Variable("America");
            M1 = new Variable("M1");
            West = new Variable("West");
            var fb =new FactBase();
            Clause c1 = new Clause();
            c1.Addp(new Predicate("American", new List<object> { x }), false);
            c1.Addp(new Predicate("Weapon", new List<object> { y }), false);
            c1.Addp(new Predicate("Sells", new List<object> { x,y,z }), false);
            c1.Addp(new Predicate("Hostile", new List<object> { z }), false);
            c1.Setq(new Predicate("Criminal", new List<object> { x }), false);
            Console.WriteLine(c1.stringify());
            Clause c2 = new Clause();
            c2.Addp(new Predicate("Owns", new List<object> { "Nono", M1 }), false);
            Console.WriteLine(c2.stringify());
            Clause c3 = new Clause();
            c3.Addp(new Predicate("Missile", new List<object> { M1 }), false);
            Console.WriteLine(c3.stringify());
            Clause c4 = new Clause();
            c4.Addp(new Predicate("Missile", new List<object> { x }), false);
            c4.Addp(new Predicate("Owns", new List<object> { "Nono", x }), false);
            c4.Setq(new Predicate("Sells", new List<object> { "West", x, "Nono" }), false);
            Console.WriteLine(c4.stringify());
            Clause c5 = new Clause();
            c5.Addp(new Predicate("Missile", new List<object> { x }), false);
            c5.Setq(new Predicate("Weapon", new List<object> { x }), false);
            Console.WriteLine(c5.stringify());
            Clause c6 = new Clause();
            c6.Addp(new Predicate("Enemy", new List<object> { x, "America" }), false);
            c6.Setq(new Predicate("Hostile", new List<object> { x }), false);
            Console.WriteLine(c6.stringify());
            Clause c7 = new Clause();
            c7.Addp(new Predicate("American", new List<object> { "West" }), false);
            Console.WriteLine(c7.stringify());
            Clause c8 = new Clause();
            c8.Addp(new Predicate("Enemy", new List<object> { "Nono", "America" }), false);
            Console.WriteLine(c8.stringify());
            fb.AddClause(c1);
            fb.AddClause(c2);
            fb.AddClause(c3);
            fb.AddClause(c4);
            fb.AddClause(c5);
            fb.AddClause(c6);
            fb.AddClause(c7);
            fb.AddClause(c8);
            Console.WriteLine();
                  // Propozitie atomica pe care dorim sa o verificam
                  var alpha =new Predicate("Criminal", new List<object> { West });
                  var fol = new InferenceEngine();
                  // Apelam algoritmul FOL-FC-ASK
                  var substitution =fol.FOL_FC_ASK(fb,alpha);
                  //Console.WriteLine(substitution);
                  // Verificam rezultatul
                 /* if (substitution != null)
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
                  }*/
            Console.Read();
        }
    }
}
