using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public abstract class ConverterBase : IConverterBase
    {
        protected const char _endChar = ',', _fullEndChar = ';', _endObjectChar = '}';
        protected readonly IReadOnlyList<char> _endChars = new char[]{_endChar, _fullEndChar, _endObjectChar};

        public abstract T Convert<T>(StringReader cson, ITypeNameResolver typeNameResolver));
        public abstract bool IsCanConvertable(StringReader cson);

        protected bool IsAnyEndChar(char value) => value == _endChar || value == _fullEndChar || value == _endObjectChar;
    }
}
