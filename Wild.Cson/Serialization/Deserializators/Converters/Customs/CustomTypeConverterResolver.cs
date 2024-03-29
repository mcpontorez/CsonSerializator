﻿using System;
using Wild.Cson.Serialization.Utils;
using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public class CustomTypeConverterResolver : IConverterResolver
    {
        private readonly IConverterResolver _mainConverterResolver;
        private readonly ICustomTypeConverterCollection _customTypeConverters;

        public CustomTypeConverterResolver(IConverterResolver mainConverterResolver)
        {
            _mainConverterResolver = mainConverterResolver;
            _customTypeConverters = 
                new CustomTypeConverterCollection(new DictionaryConverter(_mainConverterResolver), new CollectionConverter(_mainConverterResolver), new ObjectConverter(_mainConverterResolver));
        }

        public TResult Convert<TResult>(ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            cson.SkipStartsWith(StringConsts.New).SkipWhileSeparators().SkipIfNeeds(CharConsts.AtSign);
            string typeName = cson.TakeUntil(CharConsts.BeginedBrace);
            Type resultType = typeResolver.Convert(typeName);

            TResult result = _customTypeConverters.Get(resultType).Convert<TResult>(resultType, cson, typeResolver, typeMemberService);

            return result;
        }

        public bool IsConvertable(ICsonReader cson) => cson.StartsWith(StringConsts.New);
    }
}
