using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using SerializatorApp.Serialization.Utils;
using SerializatorApp.Serialization.Deserializators.Reading;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
{
    public class ObjectConverter : ICustomTypeConverter
    {
        private readonly IConverterResolver _mainConverterResolver;

        public ObjectConverter(IConverterResolver mainConverterResolver) => _mainConverterResolver = mainConverterResolver;

        public TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver)
        {
            Dictionary<string, FieldInfo> resultFieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsInitOnly).ToDictionary(f => f.Name);

            object resultValue = Activator.CreateInstance(type);

            cson.Skip(CharConsts.BeginedBrace);

            bool index = false;
            while (!cson.SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
            {
                if (index && cson.Skip(CharConsts.Comma).SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
                    break;
                else
                    index = true;

                cson.SkipIfNeeds(CharConsts.AtSign);
                string memberName = cson.TakeWhile(IsMemberNameChar);
                FieldInfo fieldInfo = resultFieldInfos[memberName];
                resultFieldInfos.Remove(memberName);

                cson.SkipWhileSeparators().Skip(CharConsts.Equal).SkipWhileSeparators();
                object fieldValue = _mainConverterResolver.Convert<object>(cson, typeResolver);
                fieldInfo.SetValue(resultValue, fieldValue);
            }

            return (TResult)resultValue;
        }

        public bool IsCanConvertable(Type type) => true;

        private static bool IsMemberNameChar(char value) => char.IsLetterOrDigit(value) || value == CharConsts.Underscore;
    }
}
