using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public abstract class ConverterBase : IConverter
    {
        protected const char _endChar = ',', _fullEndChar = ';';
        protected readonly IReadOnlyList<char> _endChars = new char[]{_endChar, _fullEndChar};

        public abstract T From<T>(StringReader cson);
        public abstract bool IsCanConvertable(StringReader cson);
    }
}
