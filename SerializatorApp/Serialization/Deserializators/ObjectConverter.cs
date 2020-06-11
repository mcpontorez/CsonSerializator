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

        private readonly IConverter _converterResolver;

        public ObjectConverter(IConverter converterResolver) => _converterResolver = converterResolver;

        public T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startString);
            cson.SkipUntil(IsTypeNameChar);

            string typeName = cson.TakeWhile(IsTypeNameChar);
            Type resultType = Type.GetType(typeName);
            IEnumerable<FieldInfo> resultFieldInfos = resultType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly);

            object resultValue = Activator.CreateInstance(resultType);

            cson.SkipWhileSeparators().SkipWhile(c => c == '{').SkipWhileSeparators();
            while (cson.CurrentChar != '}')
            {
                cson.SkipUntil(IsMemberNameChar);
                string memberName = cson.TakeWhile(IsMemberNameChar);
                FieldInfo fieldInfo = resultFieldInfos.FirstOrDefault(f => f.Name == memberName);
                if (fieldInfo == null)
                    continue;
                cson.SkipWhileSeparators().SkipWhile(c => c == '=').SkipWhileSeparators();
                object fieldValue = _converterResolver.From<object>(cson);
                fieldInfo.SetValue(resultValue, fieldValue);
            }
            cson.SkipOne();
            return (T)resultValue;
        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startString);

        private static bool IsTypeNameChar(char value) => char.IsLetter(value) || value == '.' || value == '_';
        private static bool IsMemberNameChar(char value) => char.IsLetter(value) || value == '_';
    }
}
