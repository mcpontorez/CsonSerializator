using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public class NullConverter : ConverterBase
    {
        public override T Convert<T>(CsonReader cson, ITypeResolver typeResolver)
        {
            cson.SkipStartsWith(StringConsts.Null);
            return default;
        }

        public override bool IsCanConvertable(CsonReader cson)
        {
            if (!cson.StartsWith(StringConsts.Null))
                return false;
            char postChar = cson[StringConsts.Null.Length];
            bool result = char.IsSeparator(postChar) || char.IsControl(postChar) || IsAnyEndChar(postChar);
            return result;
        }
    }
}
