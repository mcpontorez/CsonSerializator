using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using SerializatorApp.Serialization.Utils;
using SerializatorApp.Serialization.Deserializators.Reading;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
{
    public class CustomTypeConverterResolver : IConverterResolver
    {
        private readonly IConverter _mainConverterResolver;
        private readonly ICustomTypeConverterCollection _customTypeConverters;

        public CustomTypeConverterResolver(IConverter mainConverterResolver)
        {
            _mainConverterResolver = mainConverterResolver;
            _customTypeConverters = new CustomTypeConverterCollection(new ObjectConverter(_mainConverterResolver));
        }

        public TResult Convert<TResult>(CsonReader cson) => Convert<TResult>(cson, TypeResolver.InstanceWhithoutUsings);

        public TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver)
        {
            cson.SkipStartsWith(StringConsts.New).SkipWhileSeparators().SkipIfNeeds(CharConsts.AtSign);
            string typeName = cson.TakeWhile(IsTypeNameChar);
            Type resultType = typeResolver.Convert(typeName);

            TResult result = _customTypeConverters.Get(resultType).Convert<TResult>(resultType, cson, typeResolver);

            return result;
        }

        public bool IsCanConvertable(CsonReader cson) => cson.StartsWith(StringConsts.New);

        private static bool IsTypeNameChar(char c) => c != CharConsts.BeginedBrace;
    }
}
