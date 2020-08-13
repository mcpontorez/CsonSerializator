using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public interface IConverterCollection
    {
        IConverter Get(CsonReader cson);
        IConverter Get(Type type);
        bool Contains(CsonReader cson);
        bool Contains(Type type);
    }
}
