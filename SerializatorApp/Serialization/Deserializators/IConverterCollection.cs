using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface IConverterCollection
    {
        IConverter Get(StringReader cson);
        bool Contains(StringReader cson);
    }
}
