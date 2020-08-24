using Wild.Cson.Serialization.Deserializators.Reading;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public interface ICustomTypeConverter
    {
        TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver);
        bool IsCanConvertable(Type type);
    }
}
