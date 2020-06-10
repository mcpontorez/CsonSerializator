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
        private const string _startsWithValue = "new ";
        private IConverterCollection _converterCollection = new ConverterCollection(new Numerics.NumericConverterResolver());

        public T From<T>(StringReader cson)
        {
            //throw new NotImplementedException();
            cson.SkipStartsWith(_startsWithValue);
            cson.SkipWhiteSpaces();

            string typeName = cson.TakeWhile(c => char.IsLetter(c) || c == '.' || c != '_');
            Type resultType = Type.GetType(typeName);
            IEnumerable<FieldInfo> resultFieldInfos = resultType.GetFields().Where(f => !f.IsStatic && !f.IsInitOnly);

            object resultValue = Activator.CreateInstance(resultType);

            cson.SkipWhile(c => c != '{').SkipWhile(c => !char.IsLetter(c) || c == '@' || c != '_');
            while (cson.GetCurrentChar() != '}')
            {
                string memberName = cson.TakeWhile(c => char.IsLetter(c));
                FieldInfo fieldInfo = resultFieldInfos.FirstOrDefault(f => f.Name == memberName);

                fieldInfo.SetValue(resultValue, value);
            }

            return (T)resultValue;
        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startsWithValue);
    }
}
