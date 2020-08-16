using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin
{
    public interface IBuiltinTypeConverter : ICanConvertValue
    {
        TResult Convert<TResult>(CsonReader cson);
    }
}
