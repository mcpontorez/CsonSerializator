using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators
{
    public class CsonInt32Converter : ICsonConverter
    {
        public CsonData To(object source) => new CsonData(typeof(int), source.ToString());

        public CsonData To(CsData csData) => To(csData.Source);
    }
}
