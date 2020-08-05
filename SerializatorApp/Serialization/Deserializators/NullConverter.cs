using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public class NullConverter : ConverterBase
    {
        public override T Convert<T>(StringReader cson, ITypeNameResolver typeNameResolver)
        {
            cson.SkipStartsWith(StringConsts.Null);
            return default;
        }

        public override bool IsCanConvertable(StringReader cson)
        {
            if (!cson.StartsWith(StringConsts.Null))
                return false;
            char postChar = cson[StringConsts.Null.Length];
            bool result = char.IsSeparator(postChar) || char.IsControl(postChar) || IsAnyEndChar(postChar);
            return result;
        }
    }
}
