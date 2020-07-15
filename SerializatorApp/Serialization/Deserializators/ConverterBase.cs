using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public abstract class ConverterBase : IConverter
    {
        protected const char _startObjectChar = '{', _endObjectChar = '}', _endChar = ',', _fullEndChar = ';';
        protected readonly IReadOnlyList<char> _endChars = new char[]{_endChar, _fullEndChar, _endObjectChar};

        public TResult Convert<TResult>(StringReader cson) => Convert<TResult>(cson, TypeNameResolver.Empty);

        public abstract TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver);

        public abstract bool IsCanConvertable(StringReader cson);

        protected bool IsAnyEndChar(char value) => value == _endChar || value == _fullEndChar || value == _endObjectChar;
    }
}
