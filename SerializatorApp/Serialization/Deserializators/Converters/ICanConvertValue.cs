using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public interface ICanConvertValue
    {
        bool IsCanConvert(CsonReader cson);
    }
}
