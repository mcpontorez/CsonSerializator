using Wild.Cson.Serialization.Serializators.Writing;
using System;
using System.Collections;
using Wild.Cson.Serialization.Serializators.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class CollectionConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;
        public CollectionConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;

        public bool IsConvertable(object source, Type type)
        {
            if(source is IEnumerable)
            {
                return source is IList
                    || type.IsGenericType && !ReferenceEquals(type.GetInterface("System.Collections.Generic.ICollection`1"), null);
            }
            return false;
        }

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
