using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Wild.Cson.Serialization.Utils
{
    public static class StringHelper
    {
        private static readonly HashSet<string>[] _keywords = new HashSet<string>[] {
            null,           // 1 character
            new HashSet<string> {  // 2 characters
                "as",
                "do",
                "if",
                "in",
                "is",
            },
            new HashSet<string> {  // 3 characters
                "for",
                "int",
                "new",
                "out",
                "ref",
                "try",
            },
            new HashSet<string> {  // 4 characters
                "base",
                "bool",
                "byte",
                "case",
                "char",
                "else",
                "enum",
                "goto",
                "lock",
                "long",
                "null",
                "this",
                "true",
                "uint",
                "void",
            },
            new HashSet<string> {  // 5 characters
                "break",
                "catch",
                "class",
                "const",
                "event",
                "false",
                "fixed",
                "float",
                "sbyte",
                "short",
                "throw",
                "ulong",
                "using",
                "while",
            },
            new HashSet<string> {  // 6 characters
                "double",
                "extern",
                "object",
                "params",
                "public",
                "return",
                "sealed",
                "sizeof",
                "static",
                "string",
                "struct",
                "switch",
                "typeof",
                "unsafe",
                "ushort",
            },
            new HashSet<string> {  // 7 characters
                "checked",
                "decimal",
                "default",
                "finally",
                "foreach",
                "private",
                "virtual",
            },
            new HashSet<string> {  // 8 characters
                "abstract",
                "continue",
                "delegate",
                "explicit",
                "implicit",
                "internal",
                "operator",
                "override",
                "readonly",
                "volatile",
            },
            new HashSet<string> {  // 9 characters
                "__arglist",
                "__makeref",
                "__reftype",
                "interface",
                "namespace",
                "protected",
                "unchecked",
            },
            new HashSet<string> {  // 10 characters
                "__refvalue",
                "stackalloc",
            },
        };
        public static bool IsKeyword(string value)
        {
            int index = value.Length - 1;
            if (index < 1 || index > 9)
                return false;
            foreach (var c in value)
            {
                if (char.IsUpper(c))
                    return false;
            }
            return _keywords[index].Contains(value);
        }

        public static bool IsSeparatorOrAnyEndChar(this char c) => 
            IsSeparator(c) || c == CharConsts.Comma || c == CharConsts.EndedBrace || c == CharConsts.EndedSquareBracket || c == CharConsts.Semicolon;
        public static bool IsNotSeparatorOrAnyEndChar(this char c) => !IsSeparatorOrAnyEndChar(c);

        public static bool IsSeparator(this char c) => char.IsSeparator(c) || char.IsControl(c);

        public static string RemoveSeparators(this string source)
        {
            int sourceLenght = source.Length;
            int index = 0;
            Span<char> chars = sourceLenght < 128 ? stackalloc char[source.Length] : new char[source.Length];
            for (int i = 0; i < sourceLenght; i++)
            {
                char item = source[i];
                if (!item.IsSeparator())
                    chars[index++] = item;
            }
            int count = index;
            if (count != sourceLenght)
                return chars.Slice(0, count).ToString();
            else
                return source;
        }

        public static bool LastContains(this string source, char c) => source.LastIndexOf(c) != -1;
    }
}
