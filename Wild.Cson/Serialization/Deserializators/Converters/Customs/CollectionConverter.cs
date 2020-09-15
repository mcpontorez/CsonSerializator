using System;
using System.Collections.Generic;
using System.Reflection;
using Wild.Cson.Serialization.Utils;
using Wild.Cson.Serialization.Deserializators.Reading;
using System.Collections;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public class CollectionConverter : ICustomTypeConverter
    {
        private readonly IConverterResolver _mainConverterResolver;

        public CollectionConverter(IConverterResolver mainConverterResolver) => _mainConverterResolver = mainConverterResolver;

        public bool IsConvertable(Type type) => typeof(IList).IsAssignableFrom(type)
            || type.IsGenericType && !ReferenceEquals(type.GetInterface("System.Collections.Generic.ICollection`1"), null);

        public TResult Convert<TResult>(Type type, ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                    return ConvertArray<TResult>(type, cson, typeResolver, typeMemberService);
                else
                    throw new NotImplementedException($"Multidimension array is not implemented");
            }
            if (typeof(IList).GetTypeInfo().IsAssignableFrom(type))
                return ConvertList<TResult>(type, cson, typeResolver, typeMemberService);

            return ConvertGenericCollection<TResult>(type, cson, typeResolver, typeMemberService);
        }

        private TResult ConvertArray<TResult>(Type type, ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            List<object> items = new List<object>();
            IEnumerator<object> enumerator = ConvertEnumerable<object>(cson, typeResolver, typeMemberService);

            while (enumerator.MoveNext())
                items.Add(enumerator.Current);

            Array resultArray = Array.CreateInstance(type.GetElementType(), items.Count);
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray.SetValue(items[i], i);
            }

            return resultArray.WildCast<TResult>();
        }

        private TResult ConvertList<TResult>(Type type, ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            IList resultList = (IList)Activator.CreateInstance(type);

            IEnumerator<object> enumerator = ConvertEnumerable<object>(cson, typeResolver, typeMemberService);

            while (enumerator.MoveNext())
                resultList.Add(enumerator.Current);

            return resultList.WildCast<TResult>();
        }

        private TResult ConvertGenericCollection<TResult>(Type type, ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            object resultCollection = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(StringConsts.Add, BindingFlags.Public | BindingFlags.Instance);

            IEnumerator<object> enumerator = ConvertEnumerable<object>(cson, typeResolver, typeMemberService);

            var itemContainer = new object[1];
            while (enumerator.MoveNext())
                methodInfo.Invoke(resultCollection, itemContainer.SetValue(0, enumerator.Current));

            return resultCollection.WildCast<TResult>();
        }

        private IEnumerator<TItem> ConvertEnumerable<TItem>(ICsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            cson.Skip(CharConsts.BeginedBrace);

            bool index = false;
            while (!cson.SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
            {
                if (index && cson.Skip(CharConsts.Comma).SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
                    break;
                else
                    index = true;

                TItem itemValue = _mainConverterResolver.Convert<TItem>(cson, typeResolver, typeMemberService);
                yield return itemValue;
            }
        }
    }
}
