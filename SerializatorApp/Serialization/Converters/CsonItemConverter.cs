using SerializatorApp.Serialization.Models;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Converters
{
    public class CsonItemConverter : ICsonConverter
    {
        private readonly Dictionary<Type, ICsonConverter> _converters = new Dictionary<Type, ICsonConverter>
        {
            { typeof(int), new CsonInt32Converter() },
            { typeof(float), new CsonSingleConverter() },
            { typeof(string), new CsonStringConverter() },
        };

        private ICsonConverter GetConverter(Type type)
        {
            _converters.TryGetValue(type, out var _converter);
            return _converter;
        }

        public T From<T>(string cson)
        {
            throw new NotImplementedException();
        }

        public CsonData To(object source)
        {
            if (source == null)
                return new CsonData(new HashSet<Type>(), "null");
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            ICsonConverter converter = GetConverter(sourceType);
            if (converter != null)
                return converter.To(source);

            HashSet<Type> types = new HashSet<Type> { sourceType };
            string cson = $"new {sourceType.FullName} {{";
            var sourceFields = sourceType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly).ToArray();
            for (int i = 0; i < sourceFields.Length; i++)
            {
                if (i > 0) cson += ", ";
                FieldInfo field = sourceFields[i];
                object fieldValue = field.GetValue(source);
                CsonData fieldData = To(fieldValue);
                cson += $"{field.Name} = {fieldData.Cson}";
                types.AddRange(fieldData.Types);
            }
            cson += "}";
            return new CsonData(types, cson);
        }
    }
}
