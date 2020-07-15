using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Deserializators
{
    public sealed class UsingConverter
    {
        private const string _startString = "using";

        public HashSet<string> Convert(StringReader cson)
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
        
        private string ConvertUsing(StringReader cson)
        {
            string result = cson.SkipStartsWith(_startString).SkipWhileSeparators().TakeWhile(c => char.IsLetterOrDigit(c) || c == '.');
            cson.SkipWhileSeparators().SkipOne();
            return result;
        }

        public bool IsCanConvertable(StringReader cson)
        {
            if (!cson.StartsWith(_startString))
                return false;
            char postChar = cson[_startString.Length];
            bool result = char.IsSeparator(postChar) || char.IsControl(postChar);
            return result;
        }
    }
}
