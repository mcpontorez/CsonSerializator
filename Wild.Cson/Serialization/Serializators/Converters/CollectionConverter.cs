using Wild.Cson.Serialization.Serializators.Writing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class CollectionConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;

        public CollectionConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService)
        {
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            writer.AddNew().AddType(sourceType).AddLine().AddBeginedBrace().AddTabLevel();

            bool index = false;
            foreach (var item in (IEnumerable)source)
            {
                if (index)
                    writer.AddComma();
                else index = true;
                writer.AddLine();
                _converterResolver.Convert(item, writer, typeMemberService);
            }

            writer.RemoveTabLevel().AddLine().AddEndedBrace();
        }

        public bool IsConvertable(Type type) => typeof(ICollection<>).IsAssignableFrom(type) || typeof(IList).IsAssignableFrom(type);
    }
}
