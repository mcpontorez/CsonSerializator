using SerializatorApp.Serialization.Serializators.Writing;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class CollectionConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;

        public CollectionConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;

        public void Convert(object source, IStringWriter writer)
        {
            if (source == null)
            {
                writer.AddNull();
                return;
            }
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            writer.AddNew().AddType(sourceType).AddLine().AddBeginedBrace().AddTabLevel();

            bool index = false;
            foreach (var item in (IEnumerable)source)
            {
                if (index)
                    writer.AddComma().AddSpace();
                else index = true;
                writer.AddLine();
                _converterResolver.Convert(item, writer);
            }

            writer.RemoveTabLevel().AddLine().AddEndedBrace();
        }

        public bool IsConvertable(TypeInfo type) => typeof(ICollection).GetTypeInfo().IsAssignableFrom(type) || typeof(ICollection<>).GetTypeInfo().IsAssignableFrom(type);
    }
}
