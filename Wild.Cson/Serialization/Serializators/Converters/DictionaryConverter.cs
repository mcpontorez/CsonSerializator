using Wild.Cson.Serialization.Serializators.Writing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class DictionaryConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;

        public DictionaryConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService)
        {
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            writer.AddNew().AddType(sourceType).AddLine().AddBeginedBrace().AddTabLevel();

            bool index = false;

            IDictionaryEnumerator enumerator = ((IDictionary)source).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (index)
                    writer.AddComma();
                else index = true;

                writer.AddLine().AddBeginedSquareBracket();
                _converterResolver.Convert(enumerator.Key, writer, typeMemberService);
                writer.AddEndedSquareBracket().AddSpace().AddEqual().AddSpace();
                _converterResolver.Convert(enumerator.Value, writer, typeMemberService);
            }

            writer.RemoveTabLevel().AddLine().AddEndedBrace();
        }

        public bool IsConvertable(TypeInfo type) => typeof(IDictionary).GetTypeInfo().IsAssignableFrom(type);
    }
}
