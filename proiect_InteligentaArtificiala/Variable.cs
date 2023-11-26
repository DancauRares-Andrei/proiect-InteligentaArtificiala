using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_InteligentaArtificiala
{
    //Clasa pentru variabila; Variabila este definita printr-un nume.
    public class Variable
    {
        public string Name { get; set; }

        public Variable(string name)
        {
            Name = name;
        }
        
        override public string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            if (obj is Variable)
                return (obj as Variable).Name == this.Name;
            return false;
        }
    }
}
