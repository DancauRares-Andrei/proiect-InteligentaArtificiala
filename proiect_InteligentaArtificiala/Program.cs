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
            Clause c1, c2, c3, c4, c5;
            Variable x, y, z, beta;
            Predicate alpha;
            Dictionary<string,string> substitution;
            x = new Variable("X");
            y = new Variable("Y");
            z = new Variable("Z");
            beta = new Variable("B");
            var kb = new KnowledgeBase();          
            var fol = new InferenceEngine();

            //Cazul 1 de test
            Console.WriteLine("Cazul 1 de test: Criminalitatea lui West:\n");
            alpha = new Predicate("Criminal", new List<object> { "West" });
            kb.AddFact(new Predicate("Owns", new List<object> { "Nono", "M1" }));
            kb.AddFact(new Predicate("Missile", new List<object> { "M1" }));
            kb.AddFact(new Predicate("American", new List<object> { "West" }));
            kb.AddFact(new Predicate("Enemy", new List<object> { "Nono", "America" }));


            c1 = new Clause();
            c1.Addp(new Predicate("American", new List<object> { x }));
            c1.Addp(new Predicate("Weapon", new List<object> { y }));
            c1.Addp(new Predicate("Sells", new List<object> { x, y, z }));
            c1.Addp(new Predicate("Hostile", new List<object> { z }));
            c1.Setq(new Predicate("Criminal", new List<object> { x }));
            c2 = new Clause();
            c2.Addp(new Predicate("Missile", new List<object> { x }));
            c2.Addp(new Predicate("Owns", new List<object> { "Nono", x }));
            c2.Setq(new Predicate("Sells", new List<object> { "West", x, "Nono" }));
            c3 = new Clause();
            c3.Addp(new Predicate("Missile", new List<object> { x }));
            c3.Setq(new Predicate("Weapon", new List<object> { x }));
            c4 = new Clause();
            c4.Addp(new Predicate("Enemy", new List<object> { x, "America" }));
            c4.Setq(new Predicate("Hostile", new List<object> { x }));

            kb.AddRule(c1);
            kb.AddRule(c2);
            kb.AddRule(c3);
            kb.AddRule(c4);

            Console.WriteLine("Fapte:");
            foreach (var f in kb.Facts)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine("\nReguli:");
            foreach (var f in kb.Rules)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine();
            // Apelam algoritmul FOL-FC-ASK pentru baza de cunostinte actuala
            substitution = fol.FOL_FC_ASK(kb, alpha);
            // Verificam rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia poate fi demonstrata.");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstratata.");
            }

            kb.Clear();
            //Cazul 2 de test
            Console.WriteLine("\n\nCazul 2 de test: Verificarea daca John este malefic:\n");
            alpha = new Predicate("Evil", new List<object> { "John" });
            kb.AddFact(new Predicate("King", new List<object> { "John" }));
            kb.AddFact(new Predicate("Greedy", new List<object> { "John" }));
            c1 = new Clause();
            c1.Addp(new Predicate("King", new List<object> { x }));
            c1.Addp(new Predicate("Greedy", new List<object> { x }));
            c1.Setq(new Predicate("Evil", new List<object> { x }));

            kb.AddRule(c1);
            Console.WriteLine("Fapte:");
            foreach (var f in kb.Facts)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine("\nReguli:");
            foreach (var f in kb.Rules)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine();
            // Apelam algoritmul FOL-FC-ASK pentru baza de cunostinte actuala
            substitution = fol.FOL_FC_ASK(kb, alpha);
            // Verificam rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia poate fi demonstrata.");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata.");
            }
            //Cazul 1 Verificarea ca o functie este bijectiva
            /* Fie f:{3,4}->{6,8}, f(3)=6 f(4)=8. Mai jos este demonstratia ca f este bijectiva. 
              Reguli:
                 Inegal(x1, x2) si Diferit(f, x1, x2) => injectiva(f)
                 Egal(f, x1, y)=>surjectiva1(f)
                 Egal(f, x2, a)=>surjectiva2(f)
                 surjectiva1(f) si surjectiva2(f) => surjectiva(f)
                 injectiva(f) si surjectiva(f) => bijectiva(f)

              Fapte
                 Inegal(3, 4)
                 Diferit(f, 3, 4)
                 Egal(f, 3, 6)
                 Egal(f, 4, 8)
             Concluzie
                 bijectiva(f)*/
            Console.WriteLine("\n\nCazul 1 verificarea bijectivitatii unei functii\n");
            kb.Clear();
            kb.AddFact(new Predicate("Inegal", new List<object> { "3", "4" }));
            kb.AddFact(new Predicate("Diferit", new List<object> { "f", "3", "4" }));
            kb.AddFact(new Predicate("Egal", new List<object> { "f", "3", "6" }));
            kb.AddFact(new Predicate("Egal", new List<object> { "f", "4", "8" }));
            alpha = new Predicate("bijectiva", new List<object> { "f" });

            x.Name = "X1";
            y.Name = "X2";
            z.Name = "Y";
            var a = new Variable("f");
            c1 = new Clause();
            c1.Addp(new Predicate("Inegal", new List<object> { x, y }));
            c1.Addp(new Predicate("Diferit", new List<object> { a, x, y }));
            c1.Setq(new Predicate("injectiva", new List<object> { a }));
            kb.AddRule(c1);
            c2 = new Clause();
            c2.Addp(new Predicate("Egal", new List<object> { a, x, z }));
            c2.Setq(new Predicate("surjectiva1", new List<object> { a }));
            kb.AddRule(c2);
            c3 = new Clause();
            c3.Addp(new Predicate("Egal", new List<object> { a, y, beta }));
            c3.Addp(new Predicate("Inegal", new List<object> { x, y }));
            c3.Setq(new Predicate("surjectiva2", new List<object> { a }));
            kb.AddRule(c3);
            c4 = new Clause();
            c4.Addp(new Predicate("surjectiva1", new List<object> { a }));
            c4.Addp(new Predicate("surjectiva2", new List<object> { a }));
            c4.Setq(new Predicate("surjectiva", new List<object> { a }));
            kb.AddRule(c4);
            c5 = new Clause();
            c5.Addp(new Predicate("injectiva", new List<object> { a }));
            c5.Addp(new Predicate("surjectiva", new List<object> { a }));
            c5.Setq(new Predicate("bijectiva", new List<object> { a }));
            kb.AddRule(c5);
            Console.WriteLine("Fapte:");
            foreach (var f in kb.Facts)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine("\nReguli:");
            foreach (var f in kb.Rules)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine();
            // Apelam algoritmul FOL-FC-ASK pentru baza de cunostinte actuala
            substitution = fol.FOL_FC_ASK(kb, alpha);
            // Verificam rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia poate fi demonstrata.");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata.");
            }
            //Cazul 2 Lema clestelui
            /*
             Daca xn, yn si zn sunt 3 siruri astfel incat 
            xn<=yn<=zn si lim xn cand n->inf e a si lim zn cand n->inf e a
            atunci lim yn cand n->inf e a
            Fapte
            lim(xn,a)
            lim(zn,a)
            MaiMare(yn,xn) 
            MaiMare(zn,yn)

            Reguli
            lim(xn,a)=>valoare(xn,infinit,a)
            MaiMare(yn,xn) si MaiMare(zn,yn) si lim(xn,a) si lim(zn,a) => lim(yn,a)            
             */

            kb.Clear();
            Console.WriteLine("\n\nCazul 2 Lema clestelui\n");
            x.Name = "xn";
            y.Name = "yn";
            z.Name = "zn";
            beta.Name = "a";
            kb.AddFact(new Predicate("lim", new List<object> { "xn", "a" }));
            kb.AddFact(new Predicate("lim", new List<object> { "zn", "a" }));
            kb.AddFact(new Predicate("MaiMare", new List<object> { y, x }));
            kb.AddFact(new Predicate("MaiMare", new List<object> { z, y }));
            alpha = new Predicate("lim", new List<object> { y, "a" });
            c1 = new Clause();
            c1.Setq(new Predicate("valoare", new List<object> { x, "infinit", beta }));
            c1.Addp(new Predicate("lim", new List<object> { x, beta }));
            c2 = new Clause();
            c2.Addp(new Predicate("MaiMare", new List<object> { y, x }));
            c2.Addp(new Predicate("MaiMare", new List<object> { z, y }));
            c2.Addp(new Predicate("valoare", new List<object> { x, "infinit", beta }));
            c2.Addp(new Predicate("valoare", new List<object> { z, "infinit", beta }));
            c2.Setq(new Predicate("lim", new List<object> { y, beta }));
            kb.AddRule(c1);
            kb.AddRule(c2);
            Console.WriteLine("Fapte:");
            foreach (var f in kb.Facts)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine("\nReguli:");
            foreach (var f in kb.Rules)
            {
                Console.WriteLine(f.stringify());
            }
            Console.WriteLine();
            // Apelam algoritmul FOL-FC-ASK pentru baza de cunostinte actuala
            substitution = fol.FOL_FC_ASK(kb, alpha);
            // Verificam rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia poate fi demonstrata.");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata.");
            }
            Console.Read();
        }
    }
}
