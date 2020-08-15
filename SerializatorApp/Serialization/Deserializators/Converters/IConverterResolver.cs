using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public interface IConverterResolver : ICanConvertValue
    {
        TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver);
    }
}
