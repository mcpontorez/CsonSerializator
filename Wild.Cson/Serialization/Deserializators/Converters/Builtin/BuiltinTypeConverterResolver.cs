using Wild.Cson.Serialization.Deserializators.Converters.Builtin.Numerics;
using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using Wild.Cson.Serialization.Utils;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin
{
    public sealed class BuiltinTypeConverterResolver : IConverterResolver
    {
        private IBuiltinTypeConverterCollection _converters = new BuiltinTypeConverterCollection(new NullConverter(), new StringConverter(), new NumericConverterResolver());

        public bool IsConvertable(ICsonReader cson) => _converters.Contains(cson);

        public TResult Convert<TResult>(ICsonReader cson) => _converters.Get(cson).Convert<TResult>(cson);

        public TResult Convert<TResult>(ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService) => Convert<TResult>(cson);
    }
}