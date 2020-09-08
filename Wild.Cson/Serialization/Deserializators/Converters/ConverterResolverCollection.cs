﻿using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public interface IConverterResolverCollection
    {
        bool Contains(CsonReader cson);
        IConverterResolver Get(CsonReader cson);
    }

    public class ConverterResolverCollection : IConverterResolverCollection
    {
        private IEnumerable<IConverterResolver> _converterResolvers;

        public ConverterResolverCollection(IEnumerable<IConverterResolver> converterResolvers) => _converterResolvers = converterResolvers;

        public ConverterResolverCollection(params IConverterResolver[] converters) : this((IEnumerable<IConverterResolver>)converters) { }

        public bool Contains(CsonReader cson) => _converterResolvers.Any(c => c.IsConvertable(cson));

        public IConverterResolver Get(CsonReader cson) => _converterResolvers.FirstOrDefault(c => c.IsConvertable(cson));
    }
}
