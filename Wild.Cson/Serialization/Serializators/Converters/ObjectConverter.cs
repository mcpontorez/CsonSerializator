using Wild.Cson.Serialization.Serializators.Writing;
using System.Linq;
using System.Reflection;
using Wild.Cson.Serialization.Utils;
using System;
using Wild.Cson.Serialization.Serializators.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class ObjectConverter : IConverter
    {
        private readonly IConverterResolver _converterResolver;
        public ObjectConverter(IConverterResolver converterResolver) => _converterResolver = converterResolver;
        public bool IsConvertable(Type type) => true;
        public bool IsConvertable(object source, Type type) => true;

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService)
        {
            Type sourceType = source.GetType();

            writer.AddNew().AddType(sourceType).AddLine().AddBeginedBrace().AddTabLevel();
            var sourceFields = typeMemberService.GetSerializableMembers(sourceType);

            for (int i = 0; i < sourceFields.Count; i++)
            {
                if (i > 0) writer.AddComma();
                FieldInfo field = sourceFields[i];
                object fieldValue = field.GetValue(source);

                writer.AddLine().AddMemberName(field.Name).AddSpace().AddEqual().AddSpace();
                _converterResolver.Convert(fieldValue, writer, typeMemberService);
            }
            writer.RemoveTabLevel().AddLine().AddEndedBrace();
        }
    }
}
