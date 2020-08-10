using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Utils
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

        //TODO: заинлайнить
        public static bool IsSeparator(this char c) => char.IsSeparator(c) || char.IsControl(c);

        public static string RemoveSeparators(this string source)
        {
            //TODO: навернуть массив в стеке
            char[] chars = new char[source.Length];
            int index = 0;
            foreach (var c in source)
            {
                if (!IsSeparator(c))
                {
                    chars[index] = c;
                    index++;
                }
            }
            int count = index;
            if (count == source.Length)
                return source;
            else
                return new string(chars, 0, count);
        }

        public static bool LastContains(this string source, char c) => source.LastIndexOf(c) != -1;
    }
}
