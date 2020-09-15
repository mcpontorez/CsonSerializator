using Wild.Cson.Serialization.Deserializators.Converters.Builtin;
using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public sealed class MainConverterResolver : IConverterResolver
    {
        private readonly IConverterResolverCollection _converterResolvers;

        public MainConverterResolver() => _converterResolvers = new ConverterResolverCollection(new BuiltinTypeConverterResolver(), new CustomTypeConverterResolver(this));

        public TResult Convert<TResult>(ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService) => 
            (_converterResolvers.Get(cson) ?? throw new Exception($"Not found converter at symbol:{cson.Index}, cson part:\"{cson.Substring(0, 20)}...\"")).Convert<TResult>(cson, typeResolver, typeMemberService);

        public bool IsConvertable(ICsonReader cson) => _converterResolvers.Contains(cson);
    }
}
