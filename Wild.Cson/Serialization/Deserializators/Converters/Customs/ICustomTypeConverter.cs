using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using Wild.Cson.Serialization.Utils;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public interface ICustomTypeConverter
    {
        TResult Convert<TResult>(Type type, ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService);
        bool IsConvertable(Type type);
    }
}
