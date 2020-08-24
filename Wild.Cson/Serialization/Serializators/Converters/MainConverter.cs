﻿using Wild.Cson.Serialization.Serializators.Writing;
using System;
using System.Collections.Generic;
using System.Linq;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class MainConverter
    {
        private readonly IConverter _converter = new MainConverterResolver();

        public string Convert(object source)
        {
            ICsonWriter csonWriter = new CsonWriter();

            _converter.Convert(source, csonWriter, new TypeMemberService());

            string cson = csonWriter.GetString();
            return cson;
        }
    }
}
