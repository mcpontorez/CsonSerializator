using System;
using System.Collections.Generic;

namespace Wild.Cson.Serialization.Utils
{
    public static class CharConsts
    {
        public const char BeginedBrace = '{', EndedBrace = '}',
            BeginedAngleBracket = '<', EndedAngleBracket = '>',
            BeginedSquareBracket = '[', EndedSquareBracket = ']',
            Quote = '\'', DoubleQuote = '"',
            Comma = ',', Dot = '.', Semicolon = ';', Equal = '=', Space = ' ', Tab = '\t',
            AtSign = '@', Underscore = '_',
            Minus = '-';
    }
}
