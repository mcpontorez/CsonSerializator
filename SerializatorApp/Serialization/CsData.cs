using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization
{
    public ref struct CsData
    {
        public readonly object Source;
        public readonly HashSet<Type> Types;
        public readonly uint NestedLevel;
        public CsData(object source, HashSet<Type> types, uint nestedLevel)
        {
            Source = source;
            Types = types;
            NestedLevel = nestedLevel;
        }
    }
}
