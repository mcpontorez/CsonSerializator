using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators
{
    public class SingleConverter : IConverter
    {
        public CsonData To(object source) => new CsonData(typeof(float), $"{((float)source).ToString(CultureInfo.InvariantCulture)}F");

        public CsonData To(CsData csData) => To(csData.Source);

        public bool IsCanConvertable(Type type) => type == typeof(float);
    }
}
