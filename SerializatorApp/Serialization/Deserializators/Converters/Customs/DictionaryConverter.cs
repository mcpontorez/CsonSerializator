using System;
using System.Collections.Generic;
using System.Reflection;
using SerializatorApp.Serialization.Utils;
using SerializatorApp.Serialization.Deserializators.Reading;
using System.Collections;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
{
    public class DictionaryConverter : ICustomTypeConverter
    {
        private readonly IConverterResolver _mainConverterResolver;

        public DictionaryConverter(IConverterResolver mainConverterResolver) => _mainConverterResolver = mainConverterResolver;

        public bool IsCanConvertable(Type type) => typeof(IDictionary).GetTypeInfo().IsAssignableFrom(type);

        public TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            return ConvertDictionary<TResult>(type, cson, typeResolver);
        }

        private TResult ConvertDictionary<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            IDictionary resultDictionary = (IDictionary)Activator.CreateInstance(type);

            IEnumerator<KeyValuePair<object, object>> enumerator = ConvertDictionaryEnumerable<object, object>(cson, typeResolver);

            while (enumerator.MoveNext())
            {
                var kvp = enumerator.Current;
                resultDictionary.Add(kvp.Key, kvp.Value);
            }

            return resultDictionary.WildCast<TResult>();
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> ConvertDictionaryEnumerable<TKey, TValue>(CsonReader cson, ITypeResolver typeResolver)
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
                TKey key = _mainConverterResolver.Convert<TKey>(cson, typeResolver);
                cson.Skip(CharConsts.EndedSquareBracket).SkipWhileSeparators().Skip(CharConsts.Equal).SkipWhileSeparators();
                TValue value = _mainConverterResolver.Convert<TValue>(cson, typeResolver);

                yield return new KeyValuePair<TKey, TValue>(key, value);
            }
        }
    }
}
