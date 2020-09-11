using System;
using System.Collections.Generic;
using Wild.Cson.Serialization.Utils;
using Wild.Cson.Serialization.Deserializators.Reading;
using System.Collections;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public class DictionaryConverter : ICustomTypeConverter
    {
        private readonly IConverterResolver _mainConverterResolver;

        public DictionaryConverter(IConverterResolver mainConverterResolver) => _mainConverterResolver = mainConverterResolver;

        public bool IsConvertable(Type type) => typeof(IDictionary).IsAssignableFrom(type);

        public TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            return ConvertDictionary<TResult>(type, cson, typeResolver, typeMemberService);
        }

        private TResult ConvertDictionary<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            IDictionary resultDictionary = (IDictionary)Activator.CreateInstance(type);

            IEnumerator<KeyValuePair<object, object>> enumerator = ConvertDictionaryEnumerable<object, object>(cson, typeResolver, typeMemberService);

            while (enumerator.MoveNext())
            {
                var kvp = enumerator.Current;
                resultDictionary.Add(kvp.Key, kvp.Value);
            }

            return resultDictionary.WildCast<TResult>();
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> ConvertDictionaryEnumerable<TKey, TValue>(CsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            cson.Skip(CharConsts.BeginedBrace);

            bool index = false;
            while (!cson.SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
            {
                if (index && cson.Skip(CharConsts.Comma).SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
                    break;
                else
                    index = true;

                cson.Skip(CharConsts.BeginedSquareBracket).SkipWhileSeparators();
                TKey key = _mainConverterResolver.Convert<TKey>(cson, typeResolver, typeMemberService);
                cson.Skip(CharConsts.EndedSquareBracket).SkipWhileSeparators().Skip(CharConsts.Equal).SkipWhileSeparators();
                TValue value = _mainConverterResolver.Convert<TValue>(cson, typeResolver, typeMemberService);

                yield return new KeyValuePair<TKey, TValue>(key, value);
            }
        }
    }
}
