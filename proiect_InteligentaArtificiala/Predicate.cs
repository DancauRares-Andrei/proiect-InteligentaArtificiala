using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    //Clasa pentru predicate. Predicatul are un nume si o lista de argumente care poate fi compusa din variabile sau string-uri
    public class Predicate
    {
        public string Name { get; set; }
        public List<object> Arguments { get; set; }
        public Predicate()
        {
            Name = "";
            Arguments = new List<object>();
        }
        public Predicate(string name, List<object> arguments)
        {
            Name = name;
            Arguments = arguments;
        }
        public string stringify()
        {
            //Functia de transformare a unui predicat in string; Este folosita pentru a afisa la consola predicatul
            if (this.Arguments.Count == 0)
                return "";
            if (this.Arguments.Count == 1)
                return this.Name + "(" + this.Arguments[0].ToString() + ")";
            string a=this.Name+"("+this.Arguments[0].ToString();
            foreach(var arg in this.Arguments.Skip(1))
            {
                a += ", " + arg.ToString();
            }
            a += ")";
            return a;
        }
    }
}
