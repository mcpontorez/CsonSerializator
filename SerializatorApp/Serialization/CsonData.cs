using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization
{
    public ref struct CsonData
    {
        public readonly HashSet<Type> Types;
        public readonly string Cson;
        public CsonData(HashSet<Type> types, string cson)
        {
            Types = types;
            Cson = cson;
        }
    }
}
