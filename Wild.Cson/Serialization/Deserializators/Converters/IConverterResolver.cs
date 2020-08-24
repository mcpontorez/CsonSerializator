using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public interface IConverterResolver : ICanConvertValue
    {
        TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver);
    }
}
