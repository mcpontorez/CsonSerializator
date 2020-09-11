using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public sealed class UsingConverter
    {
        private const string _startString = "using";

        public HashSet<string> Convert(CsonReader cson)
        {
            HashSet<string> result = new HashSet<string>();

            do
            {
                result.Add(ConvertUsing(cson));
                cson.SkipWhileSeparators();
            }
            while (IsCanConvertable(cson));
            return result;
        }
        
        private string ConvertUsing(CsonReader cson)
        {
            string result = cson.SkipStartsWith(_startString).SkipWhileSeparators().TakeUntilSeparatorsOr(CharConsts.Semicolon);
            cson.SkipWhileSeparators().Skip(CharConsts.Semicolon);
            return result;
        }

        public bool IsCanConvertable(CsonReader cson)
        {
            if (!cson.StartsWith(_startString))
                return false;
            char postChar = cson[_startString.Length];
            bool result = postChar.IsSeparator();
            return result;
        }
    }
}
