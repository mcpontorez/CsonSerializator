using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Utils
{
    public static class StringHelper
    {
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
            return keywords[index].Contains(value);
        }

        private static readonly HashSet<string>[] keywords = new HashSet<string>[] {
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
    }
}
