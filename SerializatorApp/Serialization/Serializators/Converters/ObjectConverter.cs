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
            if (csData.Source == null)
                return new CsonData(new HashSet<Type>(), "null");
            TypeInfo sourceType = csData.Source.GetType().GetTypeInfo();

            IConverterBase converter = GetConverter(sourceType);
            if (converter != null)
                return converter.To(csData.Source);

            HashSet<Type> types = csData.Types;
            types.Add(sourceType);
            csData = new ConverterData(csData.Source, types, csData.NestedLevel);

            string cson = $"new {GetTypeName(csData)}{Environment.NewLine}{GetNestedLevelTabulation(csData.NestedLevel)}{{";
            var sourceFields = sourceType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly).ToArray();

            uint fieldNestedLevel = csData.NestedLevel + 1;
            for (int i = 0; i < sourceFields.Length; i++)
            {
                if (i > 0) cson += ", ";
                FieldInfo field = sourceFields[i];
                object fieldValue = field.GetValue(csData.Source);
                CsonData fieldData = To(new ConverterData(fieldValue, types, fieldNestedLevel));
                cson += $"{Environment.NewLine} {GetNestedLevelTabulation(fieldNestedLevel)}{field.Name} = {fieldData.Cson}";
                types.AddRange(fieldData.Types);
            }
            cson += $"{Environment.NewLine}{GetNestedLevelTabulation(csData.NestedLevel)}}}";
            return new CsonData(types, cson);
        }

        private string GetNestedLevelTabulation(uint level)
        {
            string result = string.Empty;
            for (uint i = 0; i < level; i++)
                result += '\t';
            return result;
        }
    }
}
