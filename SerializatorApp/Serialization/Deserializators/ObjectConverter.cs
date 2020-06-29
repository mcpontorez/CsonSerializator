using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace SerializatorApp.Serialization.Deserializators
{
    public class ObjectConverter : ConverterBase
    {
        private const string _startString = "new ";

        private readonly IConverter _converterResolver;

        public ObjectConverter(IConverter converterResolver) => _converterResolver = converterResolver;

        public override T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startString).SkipWhileSeparators().SkipIfNeeds('@');
            string typeName = cson.TakeWhile(IsTypeNameChar);
            Type resultType = Type.GetType(typeName);
            Dictionary<string, FieldInfo> resultFieldInfos = resultType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly).ToDictionary(f => f.Name);

            object resultValue = Activator.CreateInstance(resultType);

            cson.SkipWhileSeparators().SkipIfNeeds('(').SkipWhileSeparators().SkipIfNeeds(')').Skip('{').SkipWhileSeparators();
            while (cson.CurrentChar != _endObjectChar)
            {
                cson.SkipIfNeeds('@');
                string memberName = cson.TakeWhile(IsMemberNameChar);
                FieldInfo fieldInfo = resultFieldInfos[memberName];
                resultFieldInfos.Remove(memberName);

                cson.SkipWhileSeparators().Skip('=').SkipWhileSeparators();
                object fieldValue = _converterResolver.From<object>(cson);
                fieldInfo.SetValue(resultValue, fieldValue);

                cson.SkipWhileSeparators().SkipIfNeeds(_endChar).SkipWhileSeparators();
            }
            cson.SkipOne().SkipWhileSeparators();
            return (T)resultValue;
        }

        public override bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startString);

        private static bool IsTypeNameChar(char value) => char.IsLetter(value) || value == '.' || value == '_';
        private static bool IsMemberNameChar(char value) => char.IsLetter(value) || value == '_';
    }
}
