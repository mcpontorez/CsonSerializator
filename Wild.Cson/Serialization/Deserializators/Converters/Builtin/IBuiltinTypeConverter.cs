using Wild.Cson.Serialization.Deserializators.Reading;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin
{
    public interface IBuiltinTypeConverter : ICanConvertValue
    {
        TResult Convert<TResult>(CsonReader cson);
    }
}
