﻿using SerializatorApp.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Converters
{
    public class CsonSingleConverter : ICsonConverter
    {
        public T From<T>(string cson)
        {
            throw new NotImplementedException();
        }

        public CsonData To(object source) => new CsonData(typeof(float), $"{source}F");

        public CsonData To(CsData csData) => To(csData.Source);
    }
}
