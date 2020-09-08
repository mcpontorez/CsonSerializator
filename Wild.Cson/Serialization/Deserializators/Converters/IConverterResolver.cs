using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public interface IConverterResolver : ICanConvertValue
    {
        TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService);
    }
}
