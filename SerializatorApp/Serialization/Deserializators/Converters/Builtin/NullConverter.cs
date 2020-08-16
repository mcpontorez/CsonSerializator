using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin
{
    public class NullConverter : IBuiltinTypeConverter
    {
        public T Convert<T>(CsonReader cson)
        {
            cson.SkipStartsWith(StringConsts.Null);
            return default;
        }

        public bool IsCanConvert(CsonReader cson)
        {
            if (!cson.StartsWith(StringConsts.Null))
                return false;
            char postChar = cson[StringConsts.Null.Length];
            bool result = postChar.IsSeparatorOrAnyEndChar();
            return result;
        }
    }
}