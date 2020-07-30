using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators
{
    public class CsonInt32Converter : IConverter
    {
        public CsonData To(object source) => new CsonData(typeof(int), source.ToString());

        public CsonData To(ConverterData csData) => To(csData.Source);
    }
}
