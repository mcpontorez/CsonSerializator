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

        public bool IsConvertable(object source, Type type) => source is IList || typeof(ICollection<>).IsAssignableFrom(type);

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService)
        {
            Type sourceType = source.GetType();

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
    }
}
