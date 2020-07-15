using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface IConverterCollection
    {
        IConverterBase Get(StringReader cson);
        IConverterBase Get(Type type);
        bool Contains(StringReader cson);
        bool Contains(Type type);
    }
}
