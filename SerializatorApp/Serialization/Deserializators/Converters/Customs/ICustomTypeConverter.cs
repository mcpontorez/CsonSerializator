using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
{
    public interface ICustomTypeConverter
    {
        TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver);
        bool IsCanConvertable(Type type);
    }
}
