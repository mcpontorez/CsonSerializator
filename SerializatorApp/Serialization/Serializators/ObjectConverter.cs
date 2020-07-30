using SerializatorApp.Serialization.Serializators.Writing;
using SerializatorApp.Serialization.Utils;
using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators
{
    public class ObjectConverter : IConverter
    {
        private readonly Dictionary<Type, IConverterBase> _converters = new Dictionary<Type, IConverterBase>
        {
            { typeof(int), new CsonInt32Converter() },
            { typeof(float), new SingleConverter() },
            { typeof(string), new StringConverter() },
        };

        private IConverterBase GetConverter(Type type)
        {
            _converters.TryGetValue(type, out var _converter);
            return _converter;
        }

        public void Convert(ConverterData data, IStringWriter writer)
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
