using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Utils
{
    public static class CharConsts
    {
        public const char BeginedBrace = '{', EndedBrace = '}',
            BeginedAngleBracket = '<', EndedAngleBracket = '>',
            BeginedSquareBracket = '[', EndedSquareBracket = ']',
            Quote = '\'', DoubleQuote = '"',
            Comma = ',', Dot = '.', Semicolon = ';', Equal = '=', Space = ' ',
            AtSign = '@', Underscore = '_',
            Minus = '-';

        public static readonly IReadOnlyList<char> AnyEndChars = new char[] { Comma, EndedBrace, Semicolon };
    }
}
