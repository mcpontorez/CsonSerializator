using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Deserializators.Converters
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
            string result = cson.SkipStartsWith(_startString).SkipWhileSeparators().TakeWhile(c => char.IsLetterOrDigit(c) || c == CharConsts.Dot);
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
