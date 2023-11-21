using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    public class Predicate
    {
        public string Name { get; set; }
        public List<object> Arguments { get; set; }

        public Predicate(string name, List<object> arguments)
        {
            Name = name;
            Arguments = arguments;
        }
        public string stringify()
        {
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
