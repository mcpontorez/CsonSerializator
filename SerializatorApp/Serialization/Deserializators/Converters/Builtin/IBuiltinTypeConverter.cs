using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin
{
    public interface IBuiltinTypeConverter : ICanConvertValue
    {
        TResult Convert<TResult>(CsonReader cson);
    }
}
