using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class Int32Converter : IConverter
    {
        private const char _endChar = ';';

        public T From<T>(StringReader cson)
        {
            int endIndex = cson.TakeWhile(_endChar);
            T result = int.Parse();
        }

        public bool IsCanConvertable(StringReader cson)
        {
            int endIndex = cson.IndexOf(_endChar);
            for (int i = 0; i < endIndex; i++)
            {
                if (!char.IsDigit(cson[i]))
                    return false;
            }
            return true;
        }
    }
}
