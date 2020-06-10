using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace SerializatorApp.Serialization.Deserializators
{
    public class ObjectConverter : IConverter
    {
        private const string _startString = "new ";
        private readonly IConverter _converterResolver = new ConverterResolver();

        public T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startString);
            cson.SkipUntil(IsTypeNameChar);

            string typeName = cson.TakeWhile(IsTypeNameChar);
            Type resultType = Type.GetType(typeName);
            IEnumerable<FieldInfo> resultFieldInfos = resultType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly);

            object resultValue = Activator.CreateInstance(resultType);

            cson.SkipWhile(c => c != '{');
            while (cson.GetCurrentChar() != '}')
            {
                cson.SkipUntil(IsMemberNameChar);
                string memberName = cson.TakeWhile(IsMemberNameChar);
                FieldInfo fieldInfo = resultFieldInfos.FirstOrDefault(f => f.Name == memberName);
                if (fieldInfo == null)
                    continue;
                cson.SkipWhile(c => char.IsSeparator(c) || c == '=');
                object fieldValue = _converterResolver.From<object>(cson);
                fieldInfo.SetValue(resultValue, fieldValue);
            }
            cson.Skip(1);
            return (T)resultValue;
        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startString);

        private static bool IsTypeNameChar(char value) => char.IsLetter(value) || value == '.' || value == '_';
        private static bool IsMemberNameChar(char value) => char.IsLetter(value) || value == '_';
    }
}
