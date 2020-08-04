using SerializatorApp.Serialization.Serializators.Writing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class DictionaryConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;

        public DictionaryConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;

        public void Convert(object source, IStringWriter writer)
        {
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            writer.AddNew().AddType(sourceType).AddLine().AddBeginedBrace().AddTabLevel();

            bool index = false;

            IDictionaryEnumerator enumerator = ((IDictionary)source).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (index)
                    writer.AddComma().AddSpace();
                else index = true;

                writer.AddLine().AddBeginedSquareBracket();
                _converterResolver.Convert(enumerator.Key, writer);
                writer.AddEndedSquareBracket().AddSpace().AddEqual().AddSpace();
                _converterResolver.Convert(enumerator.Value, writer);
            }

            writer.RemoveTabLevel().AddLine().AddEndedBrace();
        }

        public bool IsConvertable(TypeInfo type) => typeof(IDictionary).GetTypeInfo().IsAssignableFrom(type);
    }
}
