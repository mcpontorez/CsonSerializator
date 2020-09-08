using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin
{
    public class NullConverter : IBuiltinTypeConverter
    {
        public T Convert<T>(CsonReader cson)
        {
            cson.SkipStartsWith(StringConsts.Null);
            return default;
        }

        public bool IsConvertable(CsonReader cson)
        {
            if (!cson.StartsWith(StringConsts.Null))
                return false;
            char postChar = cson[StringConsts.Null.Length];
            bool result = postChar.IsSeparatorOrAnyEndChar();
            return result;
        }
    }
}