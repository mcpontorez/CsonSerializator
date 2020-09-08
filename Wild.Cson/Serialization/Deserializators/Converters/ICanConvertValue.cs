using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public interface ICanConvertValue
    {
        bool IsConvertable(CsonReader cson);
    }
}
