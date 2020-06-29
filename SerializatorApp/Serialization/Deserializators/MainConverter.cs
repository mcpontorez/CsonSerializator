using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public class MainConverter : ConverterBase
    {
        private IConverter _converterResolver = new MainConverterResolver();
        public override T From<T>(StringReader cson)
        {
            cson.SkipWhileSeparators();
            return _converterResolver.From<T>(cson);
        }

        public override bool IsCanConvertable(StringReader cson) => _converterResolver.IsCanConvertable(cson.SkipWhileSeparators());
    }
}
