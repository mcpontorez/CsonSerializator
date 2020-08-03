using SerializatorApp.Serialization.Serializators.Writing;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class ObjectConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;

        public ObjectConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;

        public void Convert(object source, IStringWriter writer)
        {
            if (source == null)
            {
                writer.AddNull();
                return;
            }
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            writer.AddNew().AddType(sourceType).AddLine().AddBeginedBrace().AddTabLevel();
            var sourceFields = sourceType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly).ToArray();

            for (int i = 0; i < sourceFields.Length; i++)
            {
                if (i > 0) writer.AddComma().AddSpace();
                FieldInfo field = sourceFields[i];
                object fieldValue = field.GetValue(source);

                writer.AddLine().AddMemberName(field.Name).AddSpace().AddEqual().AddSpace();
                _converterResolver.Convert(fieldValue, writer);
            }
            writer.RemoveTabLevel().AddLine().AddEndedBrace();
        }

        public bool IsCanConvertable(Type type) => true;
    }

    class nameof { }
}
