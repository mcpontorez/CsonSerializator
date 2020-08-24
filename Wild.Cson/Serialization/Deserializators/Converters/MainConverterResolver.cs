using Wild.Cson.Serialization.Deserializators.Converters.Builtin;
using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public sealed class MainConverterResolver : IConverterResolver
    {
        private readonly IConverterResolverCollection _converterResolvers;

        public MainConverterResolver() => _converterResolvers = new ConverterResolverCollection(new BuiltinTypeConverterResolver(), new CustomTypeConverterResolver(this));

        public TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver) => 
            (_converterResolvers.Get(cson) ?? throw new Exception($"Not found converter at symbol:{cson.Index}, cson part:\"{cson.Substring(0, 20)}...\"")).Convert<TResult>(cson, typeResolver);

        public TResult Convert<TResult>(CsonReader cson) => Convert<TResult>(cson, TypeResolver.InstanceWhithoutUsings);

        public bool IsCanConvert(CsonReader cson) => _converterResolvers.Contains(cson);
    }
}
