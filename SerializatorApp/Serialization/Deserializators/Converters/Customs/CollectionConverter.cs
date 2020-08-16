﻿using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using SerializatorApp.Serialization.Utils;
using SerializatorApp.Serialization.Deserializators.Reading;
using System.Collections;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
{
    public class CollectionConverter : ICustomTypeConverter
    {
        private readonly IConverterResolver _mainConverterResolver;

        public CollectionConverter(IConverterResolver mainConverterResolver) => _mainConverterResolver = mainConverterResolver;

        public bool IsCanConvertable(Type type) => typeof(ICollection<>).GetTypeInfo().IsAssignableFrom(type) || typeof(IList).GetTypeInfo().IsAssignableFrom(type);

        public TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                    return ConvertArray<TResult>(type, cson, typeResolver);
                else
                    throw new NotImplementedException($"Multidimension array is not implemented");
            }
            if (typeof(IList).GetTypeInfo().IsAssignableFrom(type))
                return ConvertList<TResult>(type, cson, typeResolver);
        }

        private TResult ConvertArray<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            List<object> items = new List<object>();
            IEnumerator<object> enumerator = ConvertEnumerable<object>(cson, typeResolver);

            while (enumerator.MoveNext())
                items.Add(enumerator.Current);

            Array resultArray = Array.CreateInstance(type.GetElementType(), items.Count);
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray.SetValue(items[i], i);
            }

            return resultArray.SuperCast<TResult>();
        }

        private TResult ConvertList<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            var m = type.GetMethod(StringConsts.Add)

            IList resultList = (IList)Activator.CreateInstance(type);

            IEnumerator<object> enumerator = ConvertEnumerable<object>(cson, typeResolver);

            while (enumerator.MoveNext())
                resultList.Add(enumerator.Current);

            return resultList.SuperCast<TResult>();
        }

        private TResult ConvertCollection<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            IList resultList = (IList)Activator.CreateInstance(type);

            IEnumerator<object> enumerator = ConvertEnumerable<object>(cson, typeResolver);

            while (enumerator.MoveNext())
                resultList.Add(enumerator.Current);

            return resultList.SuperCast<TResult>();
        }

        private IEnumerator<TItem> ConvertEnumerable<TItem>(CsonReader cson, ITypeResolver typeResolver)
        {
            cson.Skip(CharConsts.BeginedBrace);

            bool index = false;
            while (!cson.SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
            {
                if (index && cson.Skip(CharConsts.Comma).SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
                    break;
                else
                    index = true;

                TItem itemValue = _mainConverterResolver.Convert<TItem>(cson, typeResolver);
                yield return itemValue;
            }
        }
    }
}