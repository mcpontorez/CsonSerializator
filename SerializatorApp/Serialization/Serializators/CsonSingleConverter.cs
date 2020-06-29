using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators
{
    public class CsonSingleConverter : ICsonConverter
    {
        public CsonData To(object source) => new CsonData(typeof(float), $"{source}F");

        public CsonData To(CsData csData) => To(csData.Source);
    }
}
