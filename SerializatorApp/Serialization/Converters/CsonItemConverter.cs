using SerializatorApp.Serialization.Models;
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

        public string To(object source)
        {
            if (source == null)
                return "null";
            TypeInfo sourceType = source.GetType().GetTypeInfo();

            ICsonConverter converter = GetConverter(sourceType);
            if (converter != null)
                return converter.To(source);

            string result = $"new {sourceType.FullName} {{";
            var sourceFields = sourceType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly).ToArray();
            for (int i = 0; i < sourceFields.Length; i++)
            {
                if (i > 0) result += ", ";
                FieldInfo field = sourceFields[i];
                object fieldValue = field.GetValue(source);
                result += $"{field.Name} = {To(fieldValue)}";
            }
            result += "}";
            return result;
        }
    }
}
